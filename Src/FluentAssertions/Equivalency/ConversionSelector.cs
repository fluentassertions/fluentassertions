using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Collects the members that need to be converted by the <see cref="TryConversionStep"/>. 
    /// </summary>
    public class ConversionSelector
    {
        private readonly List<Func<ISubjectInfo, bool>> inclusions = new List<Func<ISubjectInfo, bool>>();
        private readonly List<Func<ISubjectInfo, bool>> exclusions = new List<Func<ISubjectInfo, bool>>();
        private readonly StringBuilder description = new StringBuilder();

        public void IncludeAll()
        {
            inclusions.Add(_ => true);
            description.Append("Try conversion of all members. ");
        }

        public void Include(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            inclusions.Add(predicate.Compile());
            description.Append("Try conversion of member ").Append(predicate.Body).Append(". ");
        }

        public void Exclude(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            exclusions.Add(predicate.Compile());
            description.Append("Do not convert member ").Append(predicate.Body).Append(". ");
        }
        
        public bool RequiresConversion(ISubjectInfo info)
        {
            return inclusions.Any(p => p(info)) && !exclusions.Any(p => p(info));
        }

        public override string ToString()
        {
            return description.ToString();
        }
    }
}
