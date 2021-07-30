using System.Collections.Generic;

namespace CodeLuau
{
    class RegisterationFeesTable
    {
        private static Dictionary<int, int> registerationFeesTable = new Dictionary<int, int>() {
           {0, 500},  
           {1, 500},
           {2, 250},
           {3, 250},
           {4, 100},
           {5, 100},
           {6,  50},
           {7,  50},
           {8,  50},
           {9,  50},
        };

        public static int GetRegisterationFeesByExperience(int? Experience)
        {
            int _Experience = Experience ?? 0;
            return registerationFeesTable.ContainsKey(_Experience) ? registerationFeesTable[_Experience] : 0;
        }
    }
}
