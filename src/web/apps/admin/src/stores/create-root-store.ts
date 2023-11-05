import { reaction } from 'mobx';
import RootStore from './root-store';

type Props = {
  pathName: string;
};

const createRootStore = ({ pathName }: Props): RootStore => {
  const rootStore = new RootStore();

  const menuItem = rootStore.sidebarStore.menuItems.find(
    (item) => !!item.path && pathName.indexOf(item.path) !== -1
  );

  if (!menuItem) {
    rootStore.sidebarStore.setSelectedItemCode('DASHBOARD');
  } else {
    rootStore.sidebarStore.setSelectedItemCode(menuItem.code);
  }

  reaction(
    () => rootStore.sidebarStore.isSidebarOpen,
    (value) => {
      document.documentElement.style.setProperty('--sisa-sidebar-open', value ? '1' : '0');
    }
  );

  return rootStore;
};

export default createRootStore;
