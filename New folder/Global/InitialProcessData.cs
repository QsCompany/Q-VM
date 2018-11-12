using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Component;

namespace VM.Global
{
    public struct InitialProcessData
    {
        /// <summary>
        /// Instruction Pointer (begining of excecution)
        /// </summary>
        public int ip;
        /// <summary>
        /// Data Class Pointer 
        /// </summary>
        public int ebp;
        /// <summary>
        /// Data Function Stack
        /// </summary>
        public int esp;

        /// <summary>
        /// Data class Map Pointer
        /// </summary>
        public int esi;

        /// <summary>
        /// Stack Call function Pointer
        /// </summary>
        public int sc;
        /// <summary>
        /// stacl Pile Pointer
        /// </summary>
        public int ss;

        /// <summary>
        ///  Register Initializer For debugging process in pocessor
        /// </summary>
        /// <param name="ip">Instruction Pointer (begining of excecution)</param>
        /// <param name="ebp">Data Class Pointer </param>
        /// <param name="esp">Data Function Stack</param>
        /// <param name="sc">Stack Call function Pointer</param>
        /// <param name="ss">stacl Pile Pointer</param>
        /// <param name="esi">Data class Map Pointer</param>
        public InitialProcessData(int ip, int ebp, int esp, int sc, int ss, int esi)
        {
            this.ip = ip;
            this.ebp = ebp;
            this.esp = esp;
            this.esi = esi;
            this.ss = ss;
            this.sc = sc;
        }

        public void Upload(Registers register)
        {
            register[12] = ip;
            register["ebp"] = ebp;
            register["esp"] = esp;
            register["sc"] = sc;
            register["ss"] = ss;
            register["esi"] = esi;
        }
    }
}
