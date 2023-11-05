import { useContext } from 'react';

import RootStore from './root-store';

import { StoreContext } from './store-provider';

const useStore = <TStoreKey extends keyof RootStore>(key: TStoreKey): RootStore[TStoreKey] => {
  const store = useContext(StoreContext);

  if (!store) throw new Error('Global store is not available');

  return store[key];
};

export default useStore;
