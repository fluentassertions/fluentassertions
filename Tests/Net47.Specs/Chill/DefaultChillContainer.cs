using Chill;
using Chill.Autofac;

// This attribute defines which container will be used by default for this assembly

[assembly: ChillContainer(typeof(AutofacChillContainer))]

#if SILVERLIGHT
[assembly: InternalsVisibleTo("Chill")]
#endif
