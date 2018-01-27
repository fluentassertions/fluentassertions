using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Collects the members that need to be converted by the <see cref="TryConversionStep"/>.
    /// </summary>
    public class ConversionSelector
    {
        private List<Func<IMemberInfo, bool>> inclusions = new List<Func<IMemberInfo, bool>>();
        private List<Func<IMemberInfo, bool>> exclusions = new List<Func<IMemberInfo, bool>>();
        private StringBuilder description = new StringBuilder();

        public void IncludeAll()
        {
            inclusions.Add(_ => true);
            description.Append("Try conversion of all members. ");
        }

        public void Include(Expression<Func<IMemberInfo, bool>> predicate)
        {
            inclusions.Add(predicate.Compile());
            description.Append("Try conversion of member ").Append(predicate.Body).Append(". ");
        }

        public void Exclude(Expression<Func<IMemberInfo, bool>> predicate)
        {
            exclusions.Add(predicate.Compile());
            description.Append("Do not convert member ").Append(predicate.Body).Append(". ");
        }

        public bool RequiresConversion(IMemberInfo info)
        {
            return inclusions.Any(p => p(info)) && !exclusions.Any(p => p(info));
        }

        public override string ToString()
        {
            string result = description.ToString();
            return (result.Length > 0) ? result : "Without automatic conversion.";
        }

        public ConversionSelector Clone()
        {
            return new ConversionSelector
            {
                inclusions = new List<Func<IMemberInfo, bool>>(inclusions),
                exclusions = new List<Func<IMemberInfo, bool>>(exclusions),
                description = description
            };
        }
    }
}
