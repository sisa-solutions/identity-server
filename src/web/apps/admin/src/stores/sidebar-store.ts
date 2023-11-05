import { observable, computed, action, makeObservable } from 'mobx';

import MenuItemModel from './models/menu-model';

class SidebarStore {
  @observable menuItems: Array<MenuItemModel> = [];
  @observable isSidebarOpen: boolean = false;
  @observable selectedCode: string | null = null;
  @observable selectingGroupCode: string | null = null;
  @observable selectedGroupCode: string | null = null;

  @computed
  get systemGroupCodes() {
    return ['SETTINGS', 'SUPPORT'];
  }

  @computed
  get systemGroups() {
    return this.menuItems
      .filter((item) => item.parentCode === null && this.systemGroupCodes.includes(item.code))
      .sort((a, b) => a.order - b.order);
  }

  @computed
  get groups() {
    return this.menuItems
      .filter((item) => item.parentCode === null && !this.systemGroupCodes.includes(item.code))
      .sort((a, b) => a.order - b.order);
  }

  @computed
  get allItems() {
    return this.menuItems
      .filter((item) => !!item.parentCode)
      .sort((a, b) => a.order - b.order);
  }

  @computed
  get items() {
    return this.menuItems
      .filter((item) => !!item.parentCode && item.parentCode == this.selectingGroupCode)
      .sort((a, b) => a.order - b.order);
  }

  @action
  loadMenuItems(items: Array<MenuItemModel>) {
    this.menuItems = items;
  }

  @action
  setSelectingGroupCode = (code: string) => {
    const group = this.menuItems.find((item) => item.code === code);

    if (!group) {
      return;
    }

    if (group.path) {
      this.selectedCode = null;
      this.selectedGroupCode = group.code;
      this.selectingGroupCode = group.code;
      this.isSidebarOpen = false;
    } else {
      this.selectingGroupCode = code;
      this.isSidebarOpen = true;
    }
  };

  @action
  setSelectedItemCode = (code: string) => {
    const item = this.menuItems.find((item) => item.code === code);

    if (!item) {
      return;
    }

    this.isSidebarOpen = false;

    if (!item.parentCode) {
      this.selectedCode = null;
      this.selectedGroupCode = item.code;
      this.selectingGroupCode = item.code;

      return;
    }

    this.selectedCode = item.code;
    this.selectedGroupCode = item.parentCode;
    this.selectingGroupCode = item.parentCode;
  };

  @action
  toggleSidebar = () => {
    this.isSidebarOpen = !this.isSidebarOpen;
  };

  @action
  openSidebar = () => {
    this.isSidebarOpen = true;
  };

  @action
  closeSidebar = () => {
    this.isSidebarOpen = false;
  };

  constructor() {
    makeObservable(this);

    this.menuItems = [
      {
        code: 'DASHBOARD',
        name: 'Dashboard',
        icon: 'layout-dashboard',
        path: '/dashboard',
        order: 1,
        type: 'GROUP',
        parentCode: null,
      },
      {
        code: 'CATALOGS',
        name: 'Catalogs',
        icon: 'layers',
        path: null,
        order: 2,
        type: 'GROUP',
        parentCode: null,
      },
      {
        code: 'FILES',
        name: 'Files',
        icon: 'folder-tree',
        path: '/files',
        order: 3,
        type: 'ITEM',
        parentCode: null,
      },
      {
        code: 'CATEGORIES',
        name: 'Categories',
        icon: 'group',
        path: '/categories',
        order: 1,
        type: 'ITEM',
        parentCode: 'CATALOGS',
      },
      {
        code: 'SUPPORT',
        name: 'Support',
        icon: 'help-circle',
        path: "/support",
        order: 99,
        type: 'GROUP',
        parentCode: null,
      },
      {
        code: 'SETTINGS',
        name: 'Settings',
        icon: 'settings',
        path: null,
        order: 100,
        type: 'GROUP',
        parentCode: null,
      },
      {
        code: 'TAGS',
        name: 'Tags',
        icon: 'tags',
        path: '/tags',
        order: 2,
        type: 'ITEM',
        parentCode: 'CATALOGS',
      },
      {
        code: 'POSTS',
        name: 'Posts',
        icon: 'newspaper',
        path: '/posts',
        order: 3,
        type: 'ITEM',
        parentCode: 'CATALOGS',
      },
      {
        code: 'COMMENTS',
        name: 'Comments',
        icon: 'messages-square',
        path: '/comments',
        order: 4,
        type: 'ITEM',
        parentCode: 'CATALOGS',
      },
      {
        code: 'REACTIONS',
        name: 'Reactions',
        icon: 'heart',
        path: '/reactions',
        order: 5,
        type: 'ITEM',
        parentCode: 'CATALOGS',
      },
      {
        code: 'USERS',
        name: 'Users',
        icon: 'users',
        path: '/users',
        order: 1,
        type: 'ITEM',
        parentCode: 'SETTINGS',
      },
      {
        code: 'ROLES',
        name: 'Roles',
        icon: 'list-checks',
        path: '/roles',
        order: 2,
        type: 'ITEM',
        parentCode: 'SETTINGS',
      },
      {
        code: 'NOTIFICATIONS',
        name: 'Notifications',
        icon: 'bell',
        path: '/notifications',
        order: 3,
        type: 'ITEM',
        parentCode: 'SETTINGS',
      },
    ];
  }
}

export default SidebarStore;
