TOOLS     = $(HOME)/src/Ironclad/ironclad-apps/tools
BEAT      = mono $(TOOLS)/Beat/bin/beat.exe
BOOGIEASM = mono $(TOOLS)/BoogieAsm/bin/boogieasm.exe
BOOGIE    = boogie
MKDIR     = mkdir -p

OBJ    = obj/Checked/Nucleus/Base

SPEC   = ../../../Trusted/Spec/

SPECS  = $(SPEC)/base_i.bpl $(SPEC)/memory_i.bpl $(SPEC)/stacks_i.bpl $(SPEC)/assembly_i.bpl $(SPEC)/bartok_i.bpl $(SPEC)/Interrupts_i.bpl $(SPEC)/Io_i.bpl
AXIOMS = $(SPEC)/base_axioms.bpl $(SPEC)/memory_axioms.bpl $(SPEC)/stacks_axioms.bpl $(SPEC)/assembly_axioms.bpl $(SPEC)/bartok_axioms.bpl

all: build

$(OBJ):
	$(MKDIR) $(OBJ)

build: $(OBJ)
	cp Util_i.bpl $(OBJ)/Util_i.bpl
	$(BEAT) -out $(OBJ)/Util.bpl -in Util.beat

	cp Util_i.bpl $(OBJ)/Util_i.bpl
	$(BEAT) -out $(OBJ)/Util.bpl -in Util.beat

	cp $(SPEC)/Overflow_i.bpl $(OBJ)/Overflow_i.bpl
	$(BEAT) -out $(OBJ)/Overflow.bpl -in Overflow.beat
	$(BEAT) -out $(OBJ)/Separation_i.bpl -in Separation_i.beat
	$(BEAT) -out $(OBJ)/Separation.bpl   -in Separation.beat separation_i.beat

# _boogie -out $(OBJ)/Util_i.v $(SPECS) $(AXIOMS) $(OBJ)/Util_i.bpl
# _boogie -out $(OBJ)/Util.v   $(SPECS) $(AXIOMS) $(OBJ)/Util_i.bpl $(SPEC)/word_axioms.bpl $(OBJ)/Util.bpl
# _boogie -out $(OBJ)/Overflow_i.v $(SPECS) $(AXIOMS) $(OBJ)/Util_i.bpl $(OBJ)/Overflow_i.bpl
# _boogie -out $(OBJ)/Overflow.v   $(SPECS) $(AXIOMS) $(OBJ)/Util_i.bpl $(OBJ)/Overflow_i.bpl $(SPEC)/word_axioms.bpl $(OBJ)/Overflow.bpl
# _boogie -out $(OBJ)/Separation_i.v $(SPECS) $(AXIOMS) $(OBJ)/Util_i.bpl $(OBJ)/Separation_i.bpl
# _boogie -out $(OBJ)/Separation.v   $(SPECS) $(AXIOMS) $(OBJ)/Util_i.bpl $(OBJ)/Separation_i.bpl $(SPEC)/word_axioms.bpl $(OBJ)/Separation.bpl