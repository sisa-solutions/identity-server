import { Header, flexRender } from '@tanstack/react-table';

import Box from '@mui/joy/Box';
import IconButton from '@mui/joy/IconButton';

import { ArrowDownIcon, ArrowUpDownIcon, ArrowUpIcon, MinusIcon } from 'lucide-react';

import { FLEX_COLUMN_ID, SELECTION_COLUMN_ID } from './constants';
import { getPinnedClass } from './utils';

type Props<T> = {
  header: Header<T, unknown>;
};

const Th = <T extends {}>(props: Props<T>) => {
  const { header } = props;

  return (
    <Box
      component="th"
      colSpan={header.colSpan}
      className={getPinnedClass(header.column.getIsPinned())}
      sx={{
        width: header.getSize(),
        minWidth: header.getSize(),
        maxWidth: header.getSize(),
        position: 'relative',
        ...(header.id === SELECTION_COLUMN_ID && {
          textAlign: 'center',
        }),
        ...(header.id === FLEX_COLUMN_ID && {
          width: 'auto',

          borderLeft: 'none',
          borderRight: 'none',
        }),
      }}
    >
      {!header.isPlaceholder && (
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            gap: 1,
            height: '100%',
            '&:hover .data-table-sort-control > svg': {
              display: 'block',
            },
            ...(header.id === SELECTION_COLUMN_ID && {
              justifyContent: 'center',
            }),
          }}
        >
          <Box
            sx={{
              touchAction: 'none',
              cursor: 'grab',
              userSelect: 'none',
            }}
          >
            {flexRender(header.column.columnDef.header, header.getContext())}
          </Box>

          {header.column.getCanSort() && (
            <IconButton
              className="data-table-sort-control"
              variant="plain"
              color="neutral"
              size="sm"
              sx={{
                borderRadius: '50%',
                '--IconButton-size': '1.75rem',
                userSelect: 'none',
                touchAction: 'none',
                cursor: 'pointer',

                ...(!header.column.getIsSorted() && {
                  '&': {
                    opacity: 0.5,
                  },
                }),
                '& > svg': {
                  display: header.column.getIsSorted() ? 'block' : 'none',
                },
              }}
              onClick={header.column.getToggleSortingHandler()}
            >
              {{
                asc: <ArrowUpIcon />,
                desc: <ArrowDownIcon />,
              }[header.column.getIsSorted() as string] ?? <ArrowUpDownIcon />}
            </IconButton>
          )}
          {header.column.getCanResize() && (
            <Box
              sx={{
                display: 'flex',
                flexGrow: 1,
                height: '100%',
                justifyContent: 'flex-end',
                position: 'relative',

                '& > svg': {
                  background: 'transparent',
                  height: '100%',
                  cursor: 'col-resize',
                  userSelect: 'none',
                  touchAction: 'none',
                  position: 'absolute',
                  top: 0,
                  right: 'calc(-1 * var(--TableCell-paddingX))',
                },
              }}
            >
              <MinusIcon
                onMouseDown={header.getResizeHandler()}
                onTouchStart={header.getResizeHandler()}
                transform="rotate(90)"
                onClick={(evt) => {
                  evt.preventDefault();
                  evt.stopPropagation();
                }}
              />
            </Box>
          )}
        </Box>
      )}
    </Box>
  );
};

export default Th;
