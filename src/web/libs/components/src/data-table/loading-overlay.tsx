import { useMemo } from 'react';

import { Table } from '@tanstack/react-table';

import Box from '@mui/joy/Box';

import { FLEX_COLUMN_ID, SELECTION_COLUMN_ID } from './constants';
import { getPinnedClass } from './utils';

type Props<TData> = {
  table: Table<TData>;
};

const LoadingOverlay = <TData extends {}>(props: Props<TData>) => {
  const { table } = props;
  const pageSize = table.getState().pagination.pageSize;

  const fakeData = useMemo(() => Array.from({ length: pageSize }), [pageSize]);

  return (
    <Box component="tbody">
      {fakeData.map((_, i) => (
        <Box component="tr" key={i}>
          {table
            .getAllColumns()
            .filter((column) => column.getIsVisible())
            .map((column) => (
              <Box
                component="td"
                key={column.id}
                className={getPinnedClass(column.getIsPinned())}
                sx={{
                  width: column.getSize(),
                  minWidth: column.getSize(),
                  maxWidth: column.getSize(),
                  ...(column.id === SELECTION_COLUMN_ID && {
                    textAlign: 'center',
                  }),
                  ...(column.id === FLEX_COLUMN_ID && {
                    width: 'auto',

                    borderLeft: 'none',
                    borderRight: 'none',
                  }),
                }}
              >
                <Box
                  sx={(theme) => ({
                    display: 'flex',
                    flex: 1,

                    animation: 'skelenton 1s linear infinite alternate',

                    backgroundColor: theme.palette.neutral[600],
                    height: theme.spacing(2),

                    '@keyframes skelenton': {
                      '0%': {
                        opacity: 0.4,
                      },
                      '50%': {
                        opacity: 0.8,
                      },
                      '100%': {
                        opacity: 0.4,
                      },
                    },
                  })}
                />
              </Box>
            ))}
        </Box>
      ))}
    </Box>
  );
};

export default LoadingOverlay;
