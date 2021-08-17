
using AssemblyB;

namespace AssemblyA
{
    public class ClassA
    {
        public void DoSomething()
        {
            _ = new ClassB();
        }

        public ClassC ReturnClassC()
        {
            return new ClassC();
        }
    }
}
