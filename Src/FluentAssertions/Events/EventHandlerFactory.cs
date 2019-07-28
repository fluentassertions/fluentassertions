#if !NETSTANDARD1_3 && !NETSTANDARD1_6 && !NETSTANDARD2_0

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Static methods that aid in generic event subscription
    /// </summary>
    internal static class EventHandlerFactory
    {
        /// <summary>
        /// Generates an eventhandler for an event of type eventSignature that calls RegisterEvent on recorder
        /// when invoked.
        /// </summary>
        public static Delegate GenerateHandler(Type eventSignature, IEventRecorder recorder)
        {
            Type returnType = GetDelegateReturnType(eventSignature);
            Type[] parameters = GetDelegateParameterTypes(eventSignature);

            Module module = recorder.GetType()
                .GetTypeInfo()
                .Module;

            var eventHandler = new DynamicMethod(
                eventSignature.Name + "DynamicHandler",
                returnType,
                AppendParameterListThisReference(parameters),
                module);

            MethodInfo methodToCall = typeof(IEventRecorder).GetMethod("RecordEvent",
                BindingFlags.Instance | BindingFlags.Public);

            ILGenerator ilGen = eventHandler.GetILGenerator();

            // Make room for the one and only local variable in our function
            ilGen.DeclareLocal(typeof(object[]));

            // Create the object array for the parameters and store in local var index 0
            ilGen.Emit(OpCodes.Ldc_I4, parameters.Length);
            ilGen.Emit(OpCodes.Newarr, typeof(object));
            ilGen.Emit(OpCodes.Stloc_0);

            for (var index = 0; index < parameters.Length; index++)
            {
                // Push the object array onto the evaluation stack
                ilGen.Emit(OpCodes.Ldloc_0);

                // Push the array index to store our parameter in onto the evaluation stack
                ilGen.Emit(OpCodes.Ldc_I4, index);

                // Load the parameter
                ilGen.Emit(OpCodes.Ldarg, index + 1);

                // Box value-type parameters
                if (parameters[index].GetTypeInfo()
                    .IsValueType)
                {
                    ilGen.Emit(OpCodes.Box, parameters[index]);
                }

                // Store the parameter in the object array
                ilGen.Emit(OpCodes.Stelem_Ref);
            }

            // Push the this-reference on the stack as param 0 for calling the handler
            ilGen.Emit(OpCodes.Ldarg_0);

            // Push the object array onto the stack as param 1 for calling the handler
            ilGen.Emit(OpCodes.Ldloc_0);

            // Call the handler
            ilGen.EmitCall(OpCodes.Callvirt, methodToCall, null);

            ilGen.Emit(OpCodes.Ret);

            return eventHandler.CreateDelegate(eventSignature, recorder);
        }

        /// <summary>
        /// Finds the Return Type of a Delegate.
        /// </summary>
        private static Type GetDelegateReturnType(Type d)
        {
            MethodInfo invoke = DelegateInvokeMethod(d);
            return invoke.ReturnType;
        }

        /// <summary>
        /// Returns an Array of Types that make up a delegate's parameter signature.
        /// </summary>
        private static Type[] GetDelegateParameterTypes(Type d)
        {
            MethodInfo invoke = DelegateInvokeMethod(d);

            ParameterInfo[] parameterInfo = invoke.GetParameters();
            var parameters = new Type[parameterInfo.Length];

            for (var index = 0; index < parameterInfo.Length; index++)
            {
                parameters[index] = parameterInfo[index].ParameterType;
            }

            return parameters;
        }

        /// <summary>
        /// Returns an array of types appended with an EventRecorder reference at the beginning.
        /// </summary>
        private static Type[] AppendParameterListThisReference(Type[] parameters)
        {
            var newList = new Type[parameters.Length + 1];

            newList[0] = typeof(IEventRecorder);

            for (var index = 0; index < parameters.Length; index++)
            {
                newList[index + 1] = parameters[index];
            }

            return newList;
        }

        /// <summary>
        /// Returns T/F Dependent on a Type Being a Delegate.
        /// </summary>
        private static bool TypeIsDelegate(Type d)
        {
            if (d.GetTypeInfo()
                .BaseType != typeof(MulticastDelegate))
            {
                return false;
            }

            MethodInfo invoke = d.GetMethod("Invoke");
            return invoke != null;
        }

        /// <summary>
        /// Returns the MethodInfo for the Delegate's "Invoke" Method.
        /// </summary>
        private static MethodInfo DelegateInvokeMethod(Type d)
        {
            if (!TypeIsDelegate(d))
            {
                throw new ArgumentException("Type is not a Delegate!", nameof(d));
            }

            MethodInfo invoke = d.GetMethod("Invoke");
            return invoke;
        }
    }
}

#endif
