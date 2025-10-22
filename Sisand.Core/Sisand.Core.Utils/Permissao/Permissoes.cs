namespace Sisand.Core.Utils.Permissao;

public class Permissoes
{
    public static Dictionary<string, int> permissoes = new Dictionary<string, int>
    {
        { "admin", 3 },
        { "usuario", 2 },
        { "visitante", 1 }
    };
}
