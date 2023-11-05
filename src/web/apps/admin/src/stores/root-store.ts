import SidebarStore from './sidebar-store';

class RootStore {
  sidebarStore: SidebarStore;

  constructor() {
    this.sidebarStore = new SidebarStore();
  }
}

export default RootStore;
