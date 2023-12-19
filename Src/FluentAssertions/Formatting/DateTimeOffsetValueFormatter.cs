using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Formatting;

public class DateTimeOffsetValueFormatter : IValueFormatter
{
    /// <summary>
    /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanHandle(object value)
    {
        return value is DateTime or DateTimeOffset;
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Needs to be refactored")]
    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        DateTimeOffset dateTimeOffset;
        bool significantOffset = false;

        if (value is DateTime dateTime)
        {
            dateTimeOffset = dateTime.ToDateTimeOffset();
        }
        else
        {
            dateTimeOffset = (DateTimeOffset)value;
            significantOffset = true;
        }

        formattedGraph.AddFragment("<");

        bool hasDate = HasDate(dateTimeOffset);

        if (hasDate)
        {
            formattedGraph.AddFragment(dateTimeOffset.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        bool hasTime = HasTime(dateTimeOffset);

        if (hasTime)
        {
            if (hasDate)
            {
                formattedGraph.AddFragment(" ");
            }

            if (HasNanoSeconds(dateTimeOffset))
            {
                formattedGraph.AddFragment(dateTimeOffset.ToString("HH:mm:ss.fffffff", CultureInfo.InvariantCulture));
            }
            else if (HasMicroSeconds(dateTimeOffset))
            {
                formattedGraph.AddFragment(dateTimeOffset.ToString("HH:mm:ss.ffffff", CultureInfo.InvariantCulture));
            }
            else if (HasMilliSeconds(dateTimeOffset))
            {
                formattedGraph.AddFragment(dateTimeOffset.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
            }
            else
            {
                formattedGraph.AddFragment(dateTimeOffset.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
            }
        }

        if (dateTimeOffset.Offset > TimeSpan.Zero)
        {
            formattedGraph.AddFragment(" +");
            formatChild("offset", dateTimeOffset.Offset, formattedGraph);
        }
        else if (dateTimeOffset.Offset < TimeSpan.Zero)
        {
            formattedGraph.AddFragment(" ");
            formatChild("offset", dateTimeOffset.Offset, formattedGraph);
        }
        else if (significantOffset && (hasDate || hasTime))
        {
            formattedGraph.AddFragment(" +0h");
        }
        else
        {
            // No offset added, since it was deemed unnecessary
        }

        if (!hasDate && !hasTime)
        {
            if (HasMilliSeconds(dateTimeOffset))
            {
                formattedGraph.AddFragment("0001-01-01 00:00:00." + dateTimeOffset.ToString("fff", CultureInfo.InvariantCulture));
            }
            else
            {
                formattedGraph.AddFragment("0001-01-01 00:00:00.000");
            }
        }

        formattedGraph.AddFragment(">");
    }

    private static bool HasTime(DateTimeOffset dateTime)
    {
        return dateTime.Hour != 0
            || dateTime.Minute != 0
            || dateTime.Second != 0
            || HasMilliSeconds(dateTime)
            || HasMicroSeconds(dateTime)
            || HasNanoSeconds(dateTime);
    }

    private static bool HasDate(DateTimeOffset dateTime)
    {
        return dateTime.Day != 1 || dateTime.Month != 1 || dateTime.Year != 1;
    }

    private static bool HasMilliSeconds(DateTimeOffset dateTime)
    {
        return dateTime.Millisecond > 0;
    }

    private static bool HasMicroSeconds(DateTimeOffset dateTime)
    {
        return (dateTime.Ticks % TimeSpan.FromMilliseconds(1).Ticks) > 0;
    }

    private static bool HasNanoSeconds(DateTimeOffset dateTime)
    {
        return (dateTime.Ticks % (TimeSpan.FromMilliseconds(1).Ticks / 1000)) > 0;
    }
}
