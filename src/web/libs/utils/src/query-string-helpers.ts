interface SortingParams {
  id: string;
  desc: boolean;
}

export const sortParamsToString = (sortingParams: SortingParams[]) => {
  return sortingParams.map(({ id, desc }) => `${id}${desc ? ':desc' : ':asc'}`).join(',');
};
