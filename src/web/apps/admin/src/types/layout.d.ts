interface BaseMenuItem {
  code: string;
  title: string;
  icon: keyof typeof import('lucide-react/dynamicIconImports').default;
}

interface MenuGroup extends BaseMenuItem {
  path?: string;
  items?: MenuItem[];
}

interface MenuItem extends BaseMenuItem {
  groupCode: string;
  path: string;
}
