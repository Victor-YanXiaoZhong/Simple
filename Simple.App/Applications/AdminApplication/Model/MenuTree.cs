using Simple.AdminApplication.Entitys;

namespace Simple.AdminApplication.Model
{
    /// <summary>菜单树</summary>
    public class MenuTree
    {
        public MenuTree()
        {
            Childs = new List<MenuTree>();
        }

        public int Id { get; set; }

        /// <summary>菜单名称</summary>
        public string Name { get; set; }

        /// <summary>菜单地址</summary>
        public string? Url { get; set; }

        /// <summary>菜单图标</summary>
        public string? Icon { get; set; }

        /// <summary>父级Id</summary>
        public int ParentId { get; set; } = 0;

        /// <summary>是否显示菜单</summary>
        public bool IsShow { get; set; } = true;

        /// <summary>是否可关闭</summary>
        public bool IsClose { get; set; } = true;

        public int Sort { get; set; } = 0;

        public List<MenuTree> Childs { get; set; }

        public SysMenu MenuData { get; set; }

        public bool Deleted { get; set; }

        public static List<MenuTree> GetMenuTrees(List<SysMenu> menus, int parentId)
        {
            var menuTrees = new List<MenuTree>();
            var childs = menus.Where(p => p.ParentId == parentId).OrderBy(x => x.Sort).ToList();
            foreach (var child in childs)
            {
                menuTrees.Add(new MenuTree
                {
                    Id = child.Id,
                    ParentId = child.ParentId,
                    Name = child.Name,
                    Url = child.Url,
                    Icon = child.Icon,
                    Sort = child.Sort,
                    IsShow = child.IsShow,
                    IsClose = child.IsClose,
                    MenuData = child,
                    Deleted = child.Deleted,
                    Childs = GetMenuTrees(menus, child.Id)
                });
            }
            return menuTrees;
        }
    }
}