import { createContext } from 'react';

import RootStore from './root-store';

export const StoreContext = createContext<RootStore | null>(null);

const StoreProvider = StoreContext.Provider;

export default StoreProvider;
