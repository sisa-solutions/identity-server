import RootStore from './root-store';

type Props = {
  pathName: string;
};

const createRootStore = ({ pathName }: Props): RootStore => {
  const rootStore = new RootStore();

  return rootStore;
};

export default createRootStore;
