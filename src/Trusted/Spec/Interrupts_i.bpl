//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//

const ?FatalHandler:int; // Handler for fatal machine exceptions (must halt machine)
const ?ErrorHandler:int; // Handler for expected traps/faults (with error codes)
const ?FaultHandler:int; // Handler for expected traps/faults (without error codes)
const ?InterruptHandler:int; // Handler for interrupts

// Interrupt table layout:
//    0: divide error       --> FaultHandler
//    4: overflow           --> FaultHandler
//   13: general protection --> ErrorHandler
//   14: page fault         --> ErrorHandler
//   1-3, 5-12, 15-31       --> FatalHandler
//   32-255                 --> InterruptHandler
// Table contains 256 entries.
// Each entry is an 8 byte "interrupt gate" (which guarantees that exceptions are disabled upon entry):
//   0: bits 31..16 = handler[31..16]
//      bits 15...0 = 0x8e00 (P=1, DPL=0, D=1)
//   4: bits 31..16 = codeSegmentSelector
//      bits 15...0 = handler[15...0]

const ?NIdt:int; axiom ?NIdt == 256;
function IsHandlerForEntry($entry:int, $handler:int) returns(bool)
{
    ($handler == ?FaultHandler && ($entry == 0 || between(3, 5, $entry)))
 || ($handler == ?ErrorHandler && ($entry == 13 || $entry == 14))
 || ($handler == ?FatalHandler && (between(1, 3, $entry) || between(5, 13, $entry) || between(15, 32, $entry)))
 || ($handler == ?InterruptHandler && between(32, ?NIdt, $entry))
}

// REVIEW: don't hard-code the segment selector here...
const ?CodeSegmentSelector:int; axiom ?CodeSegmentSelector == 32; // 0x20 (set by boot loader)
const ?Mask16Hi:int; axiom ?Mask16Hi == 2147450880 + 2147450880; // 0xffff0000
const ?Mask16Lo:int; axiom ?Mask16Lo == 65535; // 0x0000ffff
const ?IdtWord4Lo:int; axiom ?IdtWord4Lo == 36352; // 0x8e00
function IdtWord0(handler:int) returns(int) { or(shl(?CodeSegmentSelector, 16), and(handler, ?Mask16Lo)) }
function IdtWord4(handler:int) returns(int) { or(and(handler, ?Mask16Hi), ?IdtWord4Lo) }

// Verification should ensure that:
//   - no instruction in the Nucleus can cause a fault.
//   - interrupts are always disabled in the Nucleus.
// However, it's conceivable that we could get an NMI on some hardware.
// Therefore, it seems safest to verify that the interrupt table is
// always valid, even when running Nucleus code.
// To ensure this, we reserve special memory for the interrupt table,
// and verify that only valid entries are stored there.

var $IdtMem:[int]int;
var $IdtMemOk:[int]bool;
const ?idtLo:int;
const ?idtHi:int; axiom ?idtHi == ?idtLo + ?NIdt * 8;

// Write a word to the interrupt table
procedure IdtStore($entry:int, $offset:int, $handler:int, $ptr:int, $val:int);
  requires 0 <= $entry && $entry < ?NIdt;
  requires ($offset == 0 && $val == IdtWord0($handler)) || ($offset == 4 && $val == IdtWord4($handler));
  requires IsHandlerForEntry($entry, $handler);
  requires $ptr == ?idtLo + 8 * $entry + $offset;
  modifies $Eip, $IdtMem, $IdtMemOk;
  ensures  $IdtMem == old($IdtMem)[$ptr := $val];
  ensures  $IdtMemOk == old($IdtMemOk)[$ptr := true];

function IdtMemOk($IdtMemOk:[int]bool) returns(bool)
{
  (forall i:int::{TV(i)} TV(i) ==> 0 <= i && i < ?NIdt ==>
    $IdtMemOk[?idtLo + 8 * i] && $IdtMemOk[?idtLo + 8 * i + 4])
}

var $IdtOk:bool;

// LIDT instruction.  Loads IDT register from memory.
//   $ptr + 0: 0...15: limit (size in bytes - 1)
//             16..31: addr[0...15]
//   $ptr + 4: 0...15: addr[16..31]
procedure Lidt($ptr:int);
  requires memAddr($ptr) && memAddr($ptr + 4);
  requires Aligned($ptr);
  requires $Mem[$ptr] == or(shl(?idtLo, 16), ?idtHi - ?idtLo - 1);
  requires $Mem[$ptr + 4] == shr(?idtLo, 16);
  requires IdtMemOk($IdtMemOk);
  modifies $Eip, $IdtOk;
  ensures  $IdtOk;

// Legacy programmable interrupt controller
// (VirtualPC doesn't have much APIC support yet, so use legacy PIC)
// command sequence:
//   control word, interrupt vector spec, master/slave spec, mode spec, eoi, eoi, eoi, eoi, ...
//   (eoi = end of interrupt)
var $PicSeq:[int]int; // which steps in the command sequence have we completed?

function PicOk($PicSeq:[int]int) returns(bool)
{
    $PicSeq[0] >= 5 && $PicSeq[1] >= 5
}

procedure PicOut8($pic:int, $offset:int, $seq:int);
  requires ($pic == 0 && edx == 32 + $offset) || ($pic == 1 && edx == 160 + $offset);
  requires $offset == 0 || $offset == 1;
  requires ($seq == 0 || $seq == $PicSeq[$pic] + 1);
  requires
           // edge triggered, cascaded PICs, send 4 command words:
           ($seq == 0 && $offset == 0 &&              eax == 17)
           // use interrupt vectors 112..119 and 120..127:
        || ($seq == 1 && $offset == 1 && $pic == 0 && eax == 112)
        || ($seq == 1 && $offset == 1 && $pic == 1 && eax == 120)
           // 4 ==> connect slave to IRQ2, 2 ==> slave ID 2:
        || ($seq == 2 && $offset == 1 && $pic == 0 && eax == 4)
        || ($seq == 2 && $offset == 1 && $pic == 1 && eax == 2)
           // Non-buffered, normal EOI, x86 mode:
        || ($seq == 3 && $offset == 1 &&              eax == 1)
           // for now, only allow timer interrupts (vector 0 on pic 0):
        || ($seq == 4 && $offset == 1 && $pic == 0 && eax == 254)
        || ($seq == 4 && $offset == 1 && $pic == 1 && eax == 255)
           // send eoi:
        || ($seq >= 5 && $offset == 0              && eax == 32)
        ;
  modifies $Eip, $PicSeq;
  ensures  $PicSeq == old($PicSeq)[$pic := $seq];

// Legacy programmer interval timer
// (using timer0 only)
var $TimerSeq:int; // which step in the initialization have we completed?
var $TimerFreq:int;

function TimerOk($TimerSeq:int) returns(bool) { $TimerSeq == 2 }

procedure PitModeOut8($freq:int);
  requires eax == 48; // one-shot 16-bit mode
  modifies $Eip, $TimerSeq, $TimerFreq;
  ensures  $TimerSeq == 0;
  ensures  $TimerFreq == $freq;

procedure PitFreqOut8();
  requires 0 <= $TimerSeq && $TimerSeq < 2;
  requires $TimerSeq == 0 ==> eax == $TimerFreq;         // al == bits 0-7 of $freq
  requires $TimerSeq == 1 ==> eax == shr($TimerFreq, 8); // al == bits 8-15 of $freq
  modifies $Eip, $TimerSeq;
  ensures  $TimerSeq == old($TimerSeq) + 1;
