# define the CS compiler to use
CSC = mcs
# the build target executable:
 TARGET = winexe 
#references packages
PKG = gtk-sharp-2.0 
# the target platform
PLT = x86
# embedded resource
RSR = res/icona.png res/MSSQL.png
# source files
SRC =  $(wildcard *.cs)
# reference
REF = System.Data System.Xml

default:
	$(CSC) -t:$(TARGET)	$(foreach pkg,$(PKG),$(addprefix -pkg:,$(pkg)))	-platform:$(PLT) $(foreach res,$(RSR),$(addprefix -res:,$(res))) $(foreach ref,$(REF),$(addprefix -r:,$(ref))) -out:bin/Debug/MsSqlAssistant.exe $(SRC)

clean:
	rm -f bin/Debug/*.exe 