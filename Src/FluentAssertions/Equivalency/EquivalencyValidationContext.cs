using System;

namespace FluentAssertions.Equivalency
{
    public class EquivalencyValidationContext : IEquivalencyValidationContext
    {
        private Type compileTimeType;

        public EquivalencyValidationContext()
        {
            SelectedMemberDescription = "";
            SelectedMemberPath = "";
        }

        public SelectedMemberInfo SelectedMemberInfo { get; set; }

        public string SelectedMemberPath { get; set; }

        public string SelectedMemberDescription { get; set; }

        /// <summary>
        /// Gets the value of the <see cref="IMemberInfo.SelectedMemberInfo" />
        /// </summary>
        public object Subject { get; set; }

        /// <summary>
        /// Gets the value of the <see cref="IEquivalencyValidationContext.MatchingExpectationProperty" />.
        /// </summary>
        public object Expectation { get; set; }

        /// <summary>
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        public string Because { get; set; }

        /// <summary>
        /// Zero or more objects to format using the placeholders in <see cref="IEquivalencyValidationContext.Because" />.
        /// </summary>
        public object[] BecauseArgs { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current context represents the root of the object graph.
        /// </summary>
        public bool IsRoot
        {
            get
            {
                // SMELL: That prefix should be obtained from some kind of constant
                return (SelectedMemberDescription.Length == 0) ||
                       (RootIsCollection && SelectedMemberDescription.StartsWith("item[") &&
                        !SelectedMemberDescription.Contains("."));
            }
        }

        /// <summary>
        /// Gets the compile-time type of the current expectation object. If the current object is not the root object
        /// and the type is not <see cref="object"/>,  then it returns the same <see cref="System.Type"/>
        /// as the <see cref="IMemberInfo.RuntimeType"/> property does.
        /// </summary>
        public Type CompileTimeType
        {
            get
            {
                return ((compileTimeType != typeof(object)) || Expectation is null) ? compileTimeType : RuntimeType;
            }
            set => compileTimeType = value;
        }

        /// <summary>
        /// Gets the run-time type of the current expectation object.
        /// </summary>
        public Type RuntimeType
        {
            get
            {
                if (Expectation != null)
                {
                    return Expectation.GetType();
                }

                if (SelectedMemberInfo != null)
                {
                    return SelectedMemberInfo.MemberType;
                }

                return CompileTimeType;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that the root of the graph is a collection so all type-specific options apply on
        /// the collection type and not on the root itself.
        /// </summary>
        public bool RootIsCollection { get; set; }

        public ITraceWriter Tracer { get; set; }

        public void TraceSingle(GetTraceMessage getTraceMessage)
        {
            if (Tracer != null)
            {
                string path = SelectedMemberDescription.Length > 0 ? SelectedMemberDescription : "root";
                Tracer.AddSingle(getTraceMessage(path));
            }
        }

        public IDisposable TraceBlock(GetTraceMessage getTraceMessage)
        {
            if (Tracer != null)
            {
                string path = SelectedMemberDescription.Length > 0 ? SelectedMemberDescription : "root";
                return Tracer.AddBlock(getTraceMessage(path));
            }
            else
            {
                return new Disposable(() => { });
            }
        }

        public override string ToString()
        {
            return $"{{Path=\"{SelectedMemberDescription}\", Subject={Subject}, Expectation={Expectation}}}";
        }
    }
}
