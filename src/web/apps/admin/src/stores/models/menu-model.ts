import { observable } from 'mobx';

class MenuItemModel {
  @observable code: string = '';
  @observable name: string = '';
  @observable icon: keyof typeof import('lucide-react/dynamicIconImports').default;
  @observable path: string | null = null;
  @observable order: number = 0;
  @observable type: 'OTHER' | 'GROUP' | 'ITEM' | 'SECTION' = 'OTHER';
  @observable parentCode: string | null = null;
}

export default MenuItemModel;
