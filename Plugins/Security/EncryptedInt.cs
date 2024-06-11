using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CsLibs.Security
{
    public struct EncryptedInt
    {
        private readonly int value;
        private readonly int key;
        
        public EncryptedInt(int value)
        {
            key = Random.Range(0, int.MaxValue);
            this.value = value ^ key;
        }
        
        
        public int Value => value ^ key;

        public static implicit operator int(EncryptedInt encryptedInt)
        {
            return encryptedInt.Value;
        }
        
        public static implicit operator EncryptedInt(int value)
        {
            return new EncryptedInt(value);
        }
    }
}
