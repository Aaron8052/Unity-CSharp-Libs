using System;
using System.Runtime.InteropServices;

namespace CsLibs.Collection
{
    // this array doesn't have bounds checking, use it with your own risk
    public unsafe class RawArray<T> where T : unmanaged
    {
        public RawArray(int size)
        {
            Allocate(size);
        }
        
        T* data;
        void Allocate(int size)
        {
            // alloc heap memory
            data = (T*)Marshal.AllocHGlobal(size * sizeof(T));
        }

        public T this[int index]
        {
            // return value at index
            get => *(data + index);
            
            // set value at index
            set => *(data + index) = value;
        }
        
        // dealloc 
        public void Delete()
        {
            if(data == null) 
                return;
            
            // free heap memory
            Marshal.FreeHGlobal((IntPtr)data);
            data = null;
        }
        
        // destructor: delete heap memory when object is collected by Garbage Collector
        ~RawArray()
        {
            Delete();
        }
    }
}
