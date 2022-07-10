using SmartPay.Models;

namespace SmartPay;

public static class IEnumerableExtension
{
    public static bool Contains(this IEnumerable<CheckProduct> source, Product contains)
    {
        return source.Any(s => s.Product == contains);
    }
}

