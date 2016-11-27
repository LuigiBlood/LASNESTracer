using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LASNESTracer
{
    class Program
    {
        struct SNESopcodes
        {
            public byte opcode;
            public byte totalbytes;
            public bool mode; //+1 total byte depending on mode
            public string trace;
            public string trace16;

            public SNESopcodes(byte _op, byte _total, bool _mode, string _trace)
            {
                opcode = _op;
                totalbytes = _total;
                mode = _mode;
                trace = _trace;
                trace16 = _trace;
            }

            public SNESopcodes(byte _op, byte _total, bool _mode, string _trace, string _trace16)
            {
                opcode = _op;
                totalbytes = _total;
                mode = _mode;
                trace = _trace;
                trace16 = _trace16;
            }
        }

        static SNESopcodes[] opcodeList =
        {
            //ADC
            new SNESopcodes(0x61, 2, false, "ADC (${0:X2},X)"),
            new SNESopcodes(0x63, 2, false, "ADC ${0:X2},S"),
            new SNESopcodes(0x65, 2, false, "ADC ${0:X2}"),
            new SNESopcodes(0x67, 2, false, "ADC [${0:X2}]"),
            new SNESopcodes(0x69, 2, true,  "ADC #${0:X2}", "ADC #${0:X4}"),
            new SNESopcodes(0x6D, 3, false, "ADC ${0:X4}"),
            new SNESopcodes(0x6F, 4, false, "ADC ${0:X6}"),
            new SNESopcodes(0x71, 2, false, "ADC ${0:X2},Y"),
            new SNESopcodes(0x72, 2, false, "ADC (${0:X2})"),
            new SNESopcodes(0x73, 2, false, "ADC (${0:X2},S),Y"),
            new SNESopcodes(0x75, 2, false, "ADC ${0:X2},X"),
            new SNESopcodes(0x77, 2, false, "ADC [${0:X2}],Y"),
            new SNESopcodes(0x79, 3, false, "ADC ${0:X4},Y"),
            new SNESopcodes(0x7D, 3, false, "ADC ${0:X4},X"),
            new SNESopcodes(0x7F, 4, false, "ADC ${0:X6},Y"),

            //AND
            new SNESopcodes(0x21, 2, false, "AND (${0:X2},X)"),
            new SNESopcodes(0x23, 2, false, "AND ${0:X2},S"),
            new SNESopcodes(0x25, 2, false, "AND ${0:X2}"),
            new SNESopcodes(0x27, 2, false, "AND [${0:X2}]"),
            new SNESopcodes(0x29, 2, true,  "AND #${0:X2}", "AND #${0:X4}"),
            new SNESopcodes(0x2D, 3, false, "AND ${0:X4}"),
            new SNESopcodes(0x2F, 4, false, "AND ${0:X6}"),
            new SNESopcodes(0x31, 2, false, "AND (${0:X2}),Y"),
            new SNESopcodes(0x32, 2, false, "AND (${0:X2})"),
            new SNESopcodes(0x33, 2, false, "AND (${0:X2},S),Y"),
            new SNESopcodes(0x35, 2, false, "AND ${0:X2},X"),
            new SNESopcodes(0x37, 2, false, "AND [${0:X2}],Y"),
            new SNESopcodes(0x39, 3, false, "AND ${0:X4},Y"),
            new SNESopcodes(0x3D, 3, false, "AND ${0:X4},X"),
            new SNESopcodes(0x3F, 4, false, "AND ${0:X6},X"),

            //ASL
            new SNESopcodes(0x06, 2, false, "ASL ${0:X2}"),
            new SNESopcodes(0x0A, 1, false, "ASL A"),
            new SNESopcodes(0x0E, 3, false, "ASL ${0:X4}"),
            new SNESopcodes(0x16, 2, false, "ASL ${0:X2},X"),
            new SNESopcodes(0x1E, 3, false, "ASL ${0:X4},X"),

            //BCC
            new SNESopcodes(0x90, 2, false, "BCC ${0:X2} (${1:X4})"),

            //BCS
            new SNESopcodes(0xB0, 2, false, "BCS ${0:X2} (${1:X4})"),

            //BEQ
            new SNESopcodes(0xF0, 2, false, "BEQ ${0:X2} (${1:X4})"),

            //BIT
            new SNESopcodes(0x24, 2, false, "BIT ${0:X2}"),
            new SNESopcodes(0x2C, 3, false, "BIT ${0:X4}"),
            new SNESopcodes(0x34, 2, false, "BIT ${0:X2},X"),
            new SNESopcodes(0x3C, 3, false, "BIT ${0:X4},X"),
            new SNESopcodes(0x89, 2, true,  "BIT #${0:X2}", "BIT #${0:X4}"),

            //BMI
            new SNESopcodes(0x30, 2, false, "BMI ${0:X2} (${1:X4})"),

            //BNE
            new SNESopcodes(0xD0, 2, false, "BNE ${0:X2} (${1:X4})"),

            //BPL
            new SNESopcodes(0x10, 2, false, "BPL ${0:X2} (${1:X4})"),

            //BRA
            new SNESopcodes(0x80, 2, false, "BRA ${0:X2} (${1:X4})"),

            //BRK
            new SNESopcodes(0x00, 2, false, "BRK ${0:X2}"),

            //BRL
            new SNESopcodes(0x82, 3, false, "BRL ${0:X2} (${1:X4})"),

            //BVC
            new SNESopcodes(0x50, 2, false, "BVC ${0:X2} (${1:X4})"),

            //BVS
            new SNESopcodes(0x70, 2, false, "BVS ${0:X2} (${1:X4})"),

            //CLC
            new SNESopcodes(0x18, 1, false, "CLC"),

            //CLD
            new SNESopcodes(0xD8, 1, false, "CLD"),

            //CLI
            new SNESopcodes(0x58, 1, false, "CLI"),

            //CLV
            new SNESopcodes(0xB8, 1, false, "CLV"),

            //CMP
            new SNESopcodes(0xC1, 2, false, "CMP (${0:X2},X)"),
            new SNESopcodes(0xC3, 2, false, "CMP ${0:X2},S"),
            new SNESopcodes(0xC5, 2, false, "CMP ${0:X2}"),
            new SNESopcodes(0xC7, 2, false, "CMP [${0:X2}]"),
            new SNESopcodes(0xC9, 2, true,  "CMP #${0:X2}", "CMP #${0:X4}"),
            new SNESopcodes(0xCD, 3, false, "CMP ${0:X4}"),
            new SNESopcodes(0xCF, 4, false, "CMP ${0:X6}"),
            new SNESopcodes(0xD1, 2, false, "CMP (${0:X2}),Y"),
            new SNESopcodes(0xD2, 2, false, "CMP (${0:X2})"),
            new SNESopcodes(0xD3, 2, false, "CMP (${0:X2},S),Y"),
            new SNESopcodes(0xD5, 2, false, "CMP ${0:X2},X"),
            new SNESopcodes(0xD7, 2, false, "CMP [${0:X2}],Y"),
            new SNESopcodes(0xD9, 3, false, "CMP ${0:X4},Y"),
            new SNESopcodes(0xDD, 3, false, "CMP ${0:X4},X"),
            new SNESopcodes(0xDF, 4, false, "CMP ${0:X6},X"),

            //COP
            new SNESopcodes(0x02, 2, false, "COP ${0:X2}"),

            //CPX
            new SNESopcodes(0xE0, 2, true,  "CPX #${0:X2}", "CPX #${0:X4}"),
            new SNESopcodes(0xE4, 2, false, "CPX ${0:X2}"),
            new SNESopcodes(0xEC, 3, false, "CPX ${0:X2}"),

            //CPY
            new SNESopcodes(0xC0, 2, true,  "CPY #${0:X2}", "CPY #${0:X4}"),
            new SNESopcodes(0xC4, 2, false, "CPY ${0:X2}"),
            new SNESopcodes(0xCC, 3, false, "CPY ${0:X2}"),

            //DEC
            new SNESopcodes(0x3A, 1, false, "DEC A"),
            new SNESopcodes(0xC6, 2, false, "DEC ${0:X2}"),
            new SNESopcodes(0xCE, 3, false, "DEC ${0:X4}"),
            new SNESopcodes(0xD6, 2, false, "DEC ${0:X2},X"),
            new SNESopcodes(0xDE, 3, false, "DEC ${0:X4},X"),

            //DEX
            new SNESopcodes(0xCA, 1, false, "DEX"),

            //DEY
            new SNESopcodes(0x88, 1, false, "DEY"),

            //EOR
            new SNESopcodes(0x41, 2, false, "EOR (${0:X2},X)"),
            new SNESopcodes(0x43, 2, false, "EOR ${0:X2},S"),
            new SNESopcodes(0x45, 2, false, "EOR ${0:X2}"),
            new SNESopcodes(0x47, 2, false, "EOR [${0:X2}]"),
            new SNESopcodes(0x49, 2, true,  "EOR #${0:X2}", "EOR #${0:X4}"),
            new SNESopcodes(0x4D, 3, false, "EOR ${0:X4}"),
            new SNESopcodes(0x4F, 4, false, "EOR ${0:X6}"),
            new SNESopcodes(0x51, 2, false, "EOR (${0:X2}),Y"),
            new SNESopcodes(0x52, 2, false, "EOR (${0:X2})"),
            new SNESopcodes(0x53, 2, false, "EOR (${0:X2},S),Y"),
            new SNESopcodes(0x55, 2, false, "EOR ${0:X2},X"),
            new SNESopcodes(0x57, 2, false, "EOR [${0:X2}],Y"),
            new SNESopcodes(0x59, 3, false, "EOR ${0:X4},Y"),
            new SNESopcodes(0x5D, 3, false, "EOR ${0:X4},X"),
            new SNESopcodes(0x5F, 4, false, "EOR ${0:X6},X"),

            //INC
            new SNESopcodes(0x1A, 1, false, "INC A"),
            new SNESopcodes(0xE6, 2, false, "INC ${0:X2}"),
            new SNESopcodes(0xEE, 3, false, "INC ${0:X4}"),
            new SNESopcodes(0xF6, 2, false, "INC ${0:X2},X"),
            new SNESopcodes(0xFE, 3, false, "INC ${0:X4},X"),

            //INX
            new SNESopcodes(0xE8, 1, false, "INX"),

            //INY
            new SNESopcodes(0xC8, 1, false, "INY"),

            //JMP
            new SNESopcodes(0x4C, 3, false, "JMP ${0:X4}"),
            new SNESopcodes(0x5C, 4, false, "JMP ${0:X6}"),
            new SNESopcodes(0x6C, 3, false, "JMP (${0:X4})"),
            new SNESopcodes(0x7C, 3, false, "JMP (${0:X4},X)"),
            new SNESopcodes(0xDC, 3, false, "JMP [${0:X4}]"),

            //JSR
            new SNESopcodes(0x20, 3, false, "JSR ${0:X4}"),
            new SNESopcodes(0x22, 4, false, "JSR ${0:X6}"),
            new SNESopcodes(0xFC, 3, false, "JSR (${0:X4},X)"),

            //LDA
            new SNESopcodes(0xA1, 2, false, "LDA (${0:X2},X)"),
            new SNESopcodes(0xA3, 2, false, "LDA ${0:X2},S"),
            new SNESopcodes(0xA5, 2, false, "LDA ${0:X2}"),
            new SNESopcodes(0xA7, 2, false, "LDA [${0:X2}]"),
            new SNESopcodes(0xA9, 2, true,  "LDA #${0:X2}", "LDA #${0:X4}"),
            new SNESopcodes(0xAD, 3, false, "LDA ${0:X4}"),
            new SNESopcodes(0xAF, 4, false, "LDA ${0:X6}"),
            new SNESopcodes(0xB1, 2, false, "LDA (${0:X2}),Y"),
            new SNESopcodes(0xB2, 2, false, "LDA (${0:X2})"),
            new SNESopcodes(0xB3, 2, false, "LDA (${0:X2},S),Y"),
            new SNESopcodes(0xB5, 2, false, "LDA ${0:X2},X"),
            new SNESopcodes(0xB7, 2, false, "LDA [${0:X2}],Y"),
            new SNESopcodes(0xB9, 3, false, "LDA ${0:X4},Y"),
            new SNESopcodes(0xBD, 3, false, "LDA ${0:X4},X"),
            new SNESopcodes(0xBF, 4, false, "LDA ${0:X6},X"),

            //LDX
            new SNESopcodes(0xA2, 2, true,  "LDX #${0:X2}", "LDX #${0:X4}"),
            new SNESopcodes(0xA6, 2, false, "LDX ${0:X2}"),
            new SNESopcodes(0xAE, 3, false, "LDX ${0:X4}"),
            new SNESopcodes(0xB6, 2, false, "LDX ${0:X2},Y"),
            new SNESopcodes(0xBE, 3, false, "LDX ${0:X4},Y"),

            //LDY
            new SNESopcodes(0xA0, 2, true,  "LDY #${0:X2}", "LDY #${0:X4}"),
            new SNESopcodes(0xA4, 2, false, "LDY ${0:X2}"),
            new SNESopcodes(0xAC, 3, false, "LDY ${0:X4}"),
            new SNESopcodes(0xB4, 2, false, "LDY ${0:X2},X"),
            new SNESopcodes(0xBC, 3, false, "LDY ${0:X4},X"),

            //LSR
            new SNESopcodes(0x46, 2, false, "LSR ${0:X2}"),
            new SNESopcodes(0x4A, 1, false, "LSR A"),
            new SNESopcodes(0x4E, 3, false, "LSR ${0:X4}"),
            new SNESopcodes(0x56, 2, false, "LSR ${0:X2},X"),
            new SNESopcodes(0x5E, 3, false, "LSR ${0:X4},X"),

            //MVN, MVP
            new SNESopcodes(0x54, 3, false, "MVN ${0:X2},${0:X2}"),
            new SNESopcodes(0x44, 3, false, "MVP ${0:X2},${0:X2}"),

            //NOP
            new SNESopcodes(0xEA, 1, false, "NOP"),

            //ORA
            new SNESopcodes(0x01, 2, false, "ORA (${0:X2},X)"),
            new SNESopcodes(0x03, 2, false, "ORA ${0:X2},S"),
            new SNESopcodes(0x05, 2, false, "ORA ${0:X2}"),
            new SNESopcodes(0x07, 2, false, "ORA [${0:X2}]"),
            new SNESopcodes(0x09, 2, true,  "ORA #${0:X2}", "ORA #${0:X4}"),
            new SNESopcodes(0x0D, 3, false, "ORA ${0:X4}"),
            new SNESopcodes(0x0F, 4, false, "ORA ${0:X6}"),
            new SNESopcodes(0x11, 2, false, "ORA (${0:X2}),Y"),
            new SNESopcodes(0x12, 2, false, "ORA (${0:X2})"),
            new SNESopcodes(0x13, 2, false, "ORA (${0:X2},S),Y"),
            new SNESopcodes(0x15, 2, false, "ORA ${0:X2},X"),
            new SNESopcodes(0x17, 2, false, "ORA [${0:X2}],Y"),
            new SNESopcodes(0x19, 3, false, "ORA ${0:X4},Y"),
            new SNESopcodes(0x1D, 3, false, "ORA ${0:X4},X"),
            new SNESopcodes(0x1F, 4, false, "ORA ${0:X6},X"),

            //PEA, PEI, PER
            new SNESopcodes(0xF4, 3, false, "PEA ${0:X2}"),
            new SNESopcodes(0xD4, 2, false, "PEI (${0:X2})"),
            new SNESopcodes(0x62, 3, false, "PER ${0:X2} (${0:X4})"),

            //PH?
            new SNESopcodes(0x48, 1, false, "PHA"),
            new SNESopcodes(0x8B, 1, false, "PHB"),
            new SNESopcodes(0x0B, 1, false, "PHD"),
            new SNESopcodes(0x4B, 1, false, "PHK"),
            new SNESopcodes(0x08, 1, false, "PHP"),
            new SNESopcodes(0xDA, 1, false, "PHX"),
            new SNESopcodes(0x5A, 1, false, "PHY"),

            //PL?
            new SNESopcodes(0x68, 1, false, "PLA"),
            new SNESopcodes(0xAB, 1, false, "PLB"),
            new SNESopcodes(0x2B, 1, false, "PLD"),
            new SNESopcodes(0x28, 1, false, "PLP"),
            new SNESopcodes(0xFA, 1, false, "PLX"),
            new SNESopcodes(0x7A, 1, false, "PLY"),

            //REP
            new SNESopcodes(0xC2, 2, false, "REP #${0:X2}"),

            //ROL
            new SNESopcodes(0x26, 2, false, "ROL ${0:X2}"),
            new SNESopcodes(0x2A, 1, false, "ROL A"),
            new SNESopcodes(0x2E, 3, false, "ROL ${0:X4}"),
            new SNESopcodes(0x36, 2, false, "ROL ${0:X2},X"),
            new SNESopcodes(0x3E, 3, false, "ROL ${0:X4},X"),

            //ROR
            new SNESopcodes(0x66, 2, false, "ROR ${0:X2}"),
            new SNESopcodes(0x6A, 1, false, "ROR A"),
            new SNESopcodes(0x6E, 3, false, "ROR ${0:X4}"),
            new SNESopcodes(0x76, 2, false, "ROR ${0:X2},X"),
            new SNESopcodes(0x7E, 3, false, "ROR ${0:X4},X"),

            //RT?
            new SNESopcodes(0x40, 1, false, "RTI"),
            new SNESopcodes(0x6B, 1, false, "RTL"),
            new SNESopcodes(0x60, 1, false, "RTS"),

            //SBC
            new SNESopcodes(0xE1, 2, false, "SBC (${0:X2},X)"),
            new SNESopcodes(0xE3, 2, false, "SBC ${0:X2},S"),
            new SNESopcodes(0xE5, 2, false, "SBC ${0:X2}"),
            new SNESopcodes(0xE7, 2, false, "SBC [${0:X2}]"),
            new SNESopcodes(0xE9, 2, true,  "SBC #${0:X2}", "SBC #${0:X4}"),
            new SNESopcodes(0xED, 3, false, "SBC ${0:X4}"),
            new SNESopcodes(0xEF, 4, false, "SBC ${0:X6}"),
            new SNESopcodes(0xF1, 2, false, "SBC (${0:X2}),Y"),
            new SNESopcodes(0xF2, 2, false, "SBC (${0:X2})"),
            new SNESopcodes(0xF3, 2, false, "SBC (${0:X2},S),Y"),
            new SNESopcodes(0xF5, 2, false, "SBC ${0:X2},X"),
            new SNESopcodes(0xF7, 2, false, "SBC [${0:X2}],Y"),
            new SNESopcodes(0xF9, 3, false, "SBC ${0:X4},Y"),
            new SNESopcodes(0xFD, 3, false, "SBC ${0:X4},X"),
            new SNESopcodes(0xFF, 4, false, "SBC ${0:X6},X"),

            //SE?
            new SNESopcodes(0x38, 1, false, "SEC"),
            new SNESopcodes(0xF8, 1, false, "SED"),
            new SNESopcodes(0x78, 1, false, "SEI"),
            new SNESopcodes(0xE2, 2, false, "SEP #${0:X2}"),

            //STA
            new SNESopcodes(0x81, 2, false, "STA (${0:X2},X)"),
            new SNESopcodes(0x83, 2, false, "STA ${0:X2},S"),
            new SNESopcodes(0x85, 2, false, "STA ${0:X2}"),
            new SNESopcodes(0x87, 2, false, "STA [${0:X2}]"),
            new SNESopcodes(0x8D, 3, false, "STA ${0:X4}"),
            new SNESopcodes(0x8F, 4, false, "STA ${0:X6}"),
            new SNESopcodes(0x91, 2, false, "STA (${0:X2}),Y"),
            new SNESopcodes(0x92, 2, false, "STA (${0:X2})"),
            new SNESopcodes(0x93, 2, false, "STA (${0:X2},S),Y"),
            new SNESopcodes(0x95, 2, false, "STA ${0:X2},X"),
            new SNESopcodes(0x97, 2, false, "STA [${0:X2}],Y"),
            new SNESopcodes(0x99, 3, false, "STA ${0:X4},Y"),
            new SNESopcodes(0x9D, 3, false, "STA ${0:X4},X"),
            new SNESopcodes(0x9F, 4, false, "STA ${0:X6},X"),

            //STP
            new SNESopcodes(0xDB, 1, false, "STP"),

            //STX
            new SNESopcodes(0x86, 2, false, "STX ${0:X2}"),
            new SNESopcodes(0x8E, 3, false, "STX ${0:X4}"),
            new SNESopcodes(0x96, 2, false, "STX ${0:X2},Y"),

            //STY
            new SNESopcodes(0x84, 2, false, "STY ${0:X2}"),
            new SNESopcodes(0x8C, 3, false, "STY ${0:X4}"),
            new SNESopcodes(0x94, 2, false, "STY ${0:X2},X"),

            //STZ
            new SNESopcodes(0x64, 2, false, "STZ ${0:X2}"),
            new SNESopcodes(0x74, 2, false, "STZ ${0:X2},X"),
            new SNESopcodes(0x9C, 3, false, "STZ ${0:X4}"),
            new SNESopcodes(0x9E, 3, false, "STZ ${0:X4},X"),

            //TA?
            new SNESopcodes(0xAA, 1, false, "TAX"),
            new SNESopcodes(0xA8, 1, false, "TAY"),

            //TC?
            new SNESopcodes(0x5B, 1, false, "TCD"),
            new SNESopcodes(0x1B, 1, false, "TCS"),

            //TDC
            new SNESopcodes(0x7B, 1, false, "TDC"),

            //T?B
            new SNESopcodes(0x14, 2, false, "TRB ${0:X2}"),
            new SNESopcodes(0x1C, 3, false, "TRB ${0:X4}"),
            new SNESopcodes(0x04, 2, false, "TSB ${0:X2}"),
            new SNESopcodes(0x0C, 3, false, "TSB ${0:X4}"),

            //T??
            new SNESopcodes(0x3B, 1, false, "TSC"),
            new SNESopcodes(0xBA, 1, false, "TSX"),
            new SNESopcodes(0x8A, 1, false, "TXA"),
            new SNESopcodes(0x9A, 1, false, "TXS"),
            new SNESopcodes(0x9B, 1, false, "TXY"),
            new SNESopcodes(0x98, 1, false, "TYA"),
            new SNESopcodes(0xBB, 1, false, "TYX"),

            //WAI
            new SNESopcodes(0xCB, 1, false, "WAI"),

            //WDM
            new SNESopcodes(0x42, 2, false, "WDM ${0:X2}"),

            //XBA
            new SNESopcodes(0xEB, 1, false, "XBA"),

            //XCE
            new SNESopcodes(0xFB, 1, false, "XCE"),
        };

        static bool isCART(int control)
        {
            return ((control & 8) == 0);
        }

        static bool isWR(int control)
        {
            return ((control & 4) == 0);
        }

        static bool isRD(int control)
        {
            return ((control & 2) == 0);
        }

        static int address, data, control;
        static int lastaddress, lastdata, lastcontrol;
        static bool a16, i16, WAI, opcode = false;
        static int counter = 0;
        static int imm16 = 0;
        static int opcodeaddr = 0;
        static int opcodedata1 = 0;
        static int opcodedata2 = 0;
        static int opcodedata3 = 0;
        static string temp;
        static string[] temp2;

        static StreamWriter output;

        //DMA
        static int[] DMAsize = new int[8];
        static bool[] DMAenabled = new bool[8];
        static bool DMAdelay;

        //A and I
        static int expectValue; //0 = none; 1 = write; 2 = read
        static int causeValue; //0 = A, 1 = X/Y, 2 = JSR/RTS, 3 = JSL/RTL
        static int lastaddress2, lastdata2, lastcontrol2 = 0;

        static int searchOpcode(int _opcode)
        {
            for (int i = 0; i < opcodeList.Length; i++)
            {
                if (opcodeList[i].opcode == _opcode)
                    return i;
            }
            return -1;
        }

        static bool checkOpcode(int _opcode, string _name)
        {
            string check = opcodeList[searchOpcode(_opcode)].trace.Substring(0, 3);
            return _name.Equals(check);
        }

        static bool checkOpcode(int _opcode, string[] _name)
        {
            string check = opcodeList[searchOpcode(_opcode)].trace.Substring(0, 3);
            for (int i = 0; i < _name.Length; i++)
            {
                if (_name[i].Equals(check))
                {
                    return true;
                }
            }
            return false;
        }

        static void checkValue()
        {
            if (expectValue != 0)
            {
                if (lastaddress2 == address - 1)
                    opcodedata2 |= data << (8 * counter);
                else
                    opcodedata2 = data;
                counter++;
                if (causeValue == 0 && counter >= 2)
                {
                    //A
                    if (lastaddress2 == address - 1)
                    {
                        expectValue = 0;
                        Console.WriteLine("A= " + opcodedata2.ToString("X4"));
                        output.WriteLine("A= " + opcodedata2.ToString("X4"));
                        a16 = true;
                    }
                    else
                    {
                        expectValue = 0;
                        Console.WriteLine("A= " + opcodedata2.ToString("X2"));
                        output.WriteLine("A= " + opcodedata2.ToString("X2"));
                        a16 = false;
                    }
                }
                else if (causeValue == 1 && counter >= 2)
                {
                    //X/Y
                    if (lastaddress2 == address - 1)
                    {
                        expectValue = 0;
                        Console.WriteLine("I= " + opcodedata2.ToString("X4"));
                        output.WriteLine("I= " + opcodedata2.ToString("X4"));
                        i16 = true;
                    }
                    else
                    {
                        expectValue = 0;
                        Console.WriteLine("I= " + opcodedata2.ToString("X2"));
                        output.WriteLine("I= " + opcodedata2.ToString("X2"));
                        i16 = false;
                    }
                }
                else if (causeValue == 2 && counter >= 2)
                {
                    //PC
                    expectValue = 0;
                    Console.WriteLine("PC= " + opcodedata2.ToString("X4"));
                    output.WriteLine("PC= " + opcodedata2.ToString("X4"));
                }
                else if (causeValue == 3 && counter >= 3)
                {
                    //PC
                    expectValue = 0;
                    Console.WriteLine("PC= " + opcodedata2.ToString("X6"));
                    output.WriteLine("PC= " + opcodedata2.ToString("X6"));
                }
                lastaddress2 = address;
            }
        }

        static void writeOutput()
        {
            if (checkOpcode(opcodedata1, new string[] { "BCC", "BCS", "BEQ", "BMI", "BNE", "BPL", "BRA", "BRL", "BVC", "BVS" }))
            {
                //if branch
                Console.WriteLine(opcodeaddr.ToString("X4") + ":" + String.Format(opcodeList[searchOpcode(opcodedata1)].trace, opcodedata2, opcodeaddr + 2 + (SByte)opcodedata2));
                output.WriteLine(opcodeaddr.ToString("X4") + ":" + String.Format(opcodeList[searchOpcode(opcodedata1)].trace, opcodedata2, opcodeaddr + 2 + (SByte)opcodedata2));
                return;
            }

            if (imm16 == 1)
            {
                Console.WriteLine(opcodeaddr.ToString("X4") + ":" + String.Format(opcodeList[searchOpcode(opcodedata1)].trace16, opcodedata2));
                output.WriteLine(opcodeaddr.ToString("X4") + ":" + String.Format(opcodeList[searchOpcode(opcodedata1)].trace16, opcodedata2));
            }
            else
            {
                Console.WriteLine(opcodeaddr.ToString("X4") + ":" + String.Format(opcodeList[searchOpcode(opcodedata1)].trace, opcodedata2));
                output.WriteLine(opcodeaddr.ToString("X4") + ":" + String.Format(opcodeList[searchOpcode(opcodedata1)].trace, opcodedata2));
            }
        }

        static void Main(string[] args)
        {
            //CSV dwango bus LA
            //ignore, ignore, address, data, control
            //Control: /CART, /WR, /RD, /IRQ
            Console.WriteLine("LASNESTracer 0.2 - by LuigiBlood\nUsage: LASNESTracer <filepath csv>\n--------------------");

            if (File.Exists(args[0]))
            {
                Console.WriteLine("Loading file...");
                StreamReader csv = File.OpenText(args[0]);
                output = new StreamWriter(args[0] + ".log");
                temp = csv.ReadLine(); //Read first line and ignore

                while (csv.EndOfStream == false)
                {
                    //Read line
                    temp = csv.ReadLine();
                    temp2 = temp.Split(',');
                    address = Int32.Parse(temp2[1], System.Globalization.NumberStyles.HexNumber);
                    data = Int32.Parse(temp2[2], System.Globalization.NumberStyles.HexNumber);
                    control = (Int32.Parse(temp2[3], System.Globalization.NumberStyles.HexNumber) << 1)
                        | (Int32.Parse(temp2[4], System.Globalization.NumberStyles.HexNumber) << 2)
                        | (Int32.Parse(temp2[5], System.Globalization.NumberStyles.HexNumber) << 3);

                    //Console.WriteLine(address.ToString("X4") + " : " + data.ToString("X2") + " : " + control.ToString("X2"));

                    //Process
                    if (isCART(control))
                    {
                        if (address == lastaddress)
                        {
                            WAI = true;
                        }

                        if (isRD(control))
                        {
                            checkValue();

                            if (!DMAdelay)
                            {
                                bool isDMA = false;
                                for (int i = 0; i < 8; i++)
                                {
                                    if (DMAenabled[i] && DMAsize[i] > 0)
                                    {
                                        DMAsize[i]--;
                                        if (DMAsize[i] <= 0)
                                        {
                                            DMAenabled[i] = false;
                                        }
                                        isDMA = true;
                                        break;
                                    }
                                    else
                                    {
                                        DMAenabled[i] = false;
                                    }
                                }

                                if (isDMA)
                                    continue;
                            }
                            else
                                DMAdelay = false;

                            if (address == 0xFFEB && lastaddress == 0xFFEA)
                            {
                                output.WriteLine("NMI Issued");
                            }

                            {
                                if (address - 1 != lastaddress && opcode)
                                {
                                    opcode = false;
                                    //Console.WriteLine("Not an opcode");
                                }

                                if (opcode)
                                {
                                    if (counter < opcodeList[searchOpcode(opcodedata1)].totalbytes + imm16 - 1)
                                    {
                                        opcodedata2 |= data << (8 * counter);
                                        counter++;

                                        if (opcodeList[searchOpcode(opcodedata1)].mode == true)
                                        {
                                            if (checkOpcode(opcodedata1, new string[]{ "ADC", "AND", "BIT", "CMP", "EOR", "LDA", "ORA", "SBC" }) 
                                                && a16 == true)
                                            {
                                                imm16 = 1;
                                            }

                                            if (checkOpcode(opcodedata1, new string[]{ "CPX", "CPY", "LDX", "LDY" })
                                                && i16 == true)
                                            {
                                                imm16 = 1;
                                            }
                                        }
                                        else
                                        {
                                            imm16 = 0;
                                        }
                                    }

                                    if (counter >= opcodeList[searchOpcode(opcodedata1)].totalbytes + imm16 - 1)
                                    {
                                        writeOutput();
                                        opcode = false;

                                        if (checkOpcode(opcodedata1, new string[] { "STA", "PHA" }))
                                        {
                                            expectValue = 1;
                                            causeValue = 0;
                                            counter = 0;
                                        }
                                        else if (checkOpcode(opcodedata1, new string[] { "LDA", "PLA" }))
                                        {
                                            expectValue = 2;
                                            causeValue = 0;
                                            counter = 0;
                                        }
                                        else if (checkOpcode(opcodedata1, new string[] { "STX", "STY", "PHX", "PHY" }))
                                        {
                                            expectValue = 1;
                                            causeValue = 1;
                                            counter = 0;
                                        }
                                        else if (checkOpcode(opcodedata1, new string[] { "LDX", "LDY", "PLX", "PLY" }))
                                        {
                                            expectValue = 2;
                                            causeValue = 1;
                                            counter = 0;
                                        }
                                        /*
                                        else if (checkOpcode(opcodedata1, new string[] { "JSR" }))
                                        {
                                            expectValue = 1;
                                            causeValue = 2;
                                            counter = 0;
                                        }
                                        else if (checkOpcode(opcodedata1, new string[] { "RTS" }))
                                        {
                                            expectValue = 2;
                                            causeValue = 2;
                                            counter = 0;
                                        }
                                        else if (checkOpcode(opcodedata1, new string[] { "JSL" }))
                                        {
                                            expectValue = 1;
                                            causeValue = 3;
                                            counter = 0;
                                        }
                                        else if (checkOpcode(opcodedata1, new string[] { "RTL", "RTI" }))
                                        {
                                            expectValue = 2;
                                            causeValue = 3;
                                            counter = 0;
                                        }*/
                                        else
                                            expectValue = 0;

                                        if (checkOpcode(opcodedata1, "REP"))
                                        {
                                            //REP
                                            if ((opcodedata2 & 0x20) != 0)
                                                a16 = true;
                                            if ((opcodedata2 & 0x10) != 0)
                                                i16 = true;
                                        }
                                        else if (checkOpcode(opcodedata1, "SEP"))
                                        {
                                            //SEP
                                            if ((opcodedata2 & 0x20) != 0)
                                                a16 = false;
                                            if ((opcodedata2 & 0x10) != 0)
                                                i16 = false;
                                        }
                                    }
                                    WAI = false;
                                }
                                else
                                {
                                    opcode = true;
                                    //Console.WriteLine("Opcode found");
                                    counter = 0;
                                    WAI = false;
                                    opcodeaddr = address;
                                    opcodedata1 = data;
                                    opcodedata2 = 0;
                                    opcodedata3 = 0;

                                    if (opcodeList[searchOpcode(opcodedata1)].totalbytes == 1)
                                    {
                                        imm16 = 0;
                                        writeOutput();
                                        opcode = false;
                                    }
                                }
                            }
                            //Keep last
                            lastaddress = address;
                            lastdata = data;
                            lastcontrol = control;
                        }
                        else if (isWR(control))
                        {
                            checkValue();
                        }
                        else
                        {
                            //not RD, not WR, something else is going on
                            WAI = true;
                        }
                    }
                    else
                    {
                        if (isWR(control))
                        {
                            if ((address & 0xFF0F) == 0x4305)
                            {
                                DMAsize[(address & 0xF0) >> 4] = data;
                            }
                            else if ((address & 0xFF0F) == 0x4306)
                            {
                                DMAsize[(address & 0xF0) >> 4] |= data << 8;
                            }
                            else if (address == 0x420B)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    DMAenabled[i] = ((data >> i) & 1) == 1;
                                    if (DMAenabled[i])
                                        DMAdelay = true;
                                }
                            }
                        }
                        checkValue();
                    }
                }

                output.Close();
                csv.Close();
            }
            else
            {
                Console.WriteLine("Error: File doesn't exist");
            }
        }
    }
}
