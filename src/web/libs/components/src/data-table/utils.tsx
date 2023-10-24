import { type ColumnPinningPosition } from '@tanstack/react-table';

export const getPinnedClass = (position: ColumnPinningPosition) => {
  switch (position) {
    case 'left':
      return 'data-table-pinned-left';
    case 'right':
      return 'data-table-pinned-right';
    default:
      return '';
  }
};
