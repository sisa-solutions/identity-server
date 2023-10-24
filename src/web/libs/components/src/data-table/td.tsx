import { Cell, flexRender } from '@tanstack/react-table';

import Box from '@mui/joy/Box';

import { FLEX_COLUMN_ID, SELECTION_COLUMN_ID } from './constants';
import { getPinnedClass } from './utils';

type Props<T> = {
  cell: Cell<T, unknown>;
};

const Td = <T extends {}>(props: Props<T>) => {
  const { cell } = props;

  return (
    <Box
      component="td"
      key={cell.id}
      className={getPinnedClass(cell.column.getIsPinned())}
      sx={{
        width: cell.column.getSize(),
        minWidth: cell.column.getSize(),
        maxWidth: cell.column.getSize(),
        ...(cell.column.id === SELECTION_COLUMN_ID && {
          textAlign: 'center',
        }),
        ...(cell.column.id === FLEX_COLUMN_ID && {
          width: 'auto',

          borderLeft: 'none',
          borderRight: 'none',
        }),
      }}
    >
      <Box>{flexRender(cell.column.columnDef.cell, cell.getContext())}</Box>
    </Box>
  );
};

export default Td;
