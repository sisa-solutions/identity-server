import {
  type TableOptions,
  useReactTable,
  getSortedRowModel,
  getFilteredRowModel,
  getCoreRowModel,
  getPaginationRowModel,
} from '@tanstack/react-table';

import Table from '@mui/joy/Table';

import THead from './thead';
import TBody from './tbody';
import LoadingOverlay from './loading-overlay';

export type DataTableProps<T> = Pick<
  TableOptions<T>,
  | 'data'
  | 'columns'
  | 'state'
  | 'pageCount'
  | 'manualPagination'
  | 'enableMultiSort'
  | 'enableRowSelection'
  | 'onRowSelectionChange'
  | 'onPaginationChange'
  | 'onSortingChange'
  | 'onColumnFiltersChange'
  | 'onColumnPinningChange'
  | 'onColumnOrderChange'
> & {
  itemCount: number;
  isLoading?: boolean;
  slots?: {
    toolbar?: React.ReactNode;
  };
};

const DataTable = <T extends {}>({
  isLoading,
  columns,
  data,
  state,
  pageCount,
  manualPagination,
  enableMultiSort,
  onPaginationChange,
  onRowSelectionChange,
  onSortingChange,
  onColumnFiltersChange,
  onColumnPinningChange,
  onColumnOrderChange,
}: DataTableProps<T>) => {
  const table = useReactTable({
    data,
    columns,
    state: {
      ...state,
    },
    enableRowSelection: true, //enable row selection for all rows
    enableSorting: true,
    enablePinning: true,
    columnResizeMode: 'onChange',
    pageCount,
    manualPagination: manualPagination ?? true,
    debugTable: true,
    enableMultiSort,

    onRowSelectionChange,
    onPaginationChange,
    onSortingChange,
    onColumnFiltersChange,
    onColumnPinningChange,
    onColumnOrderChange,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
  });

  // const leftHeaderGroups = table.getLeftHeaderGroups();
  // const centerHeaderGroups = table.getCenterHeaderGroups();
  // const rightHeaderGroups = table.getRightHeaderGroups();

  // const leftRowsData = table.getRowModel().rows.map((row) => ({
  //   ...row,
  //   getVisibleCells: () => row.getLeftVisibleCells(),
  // }));

  // const centerRowsData = table.getRowModel().rows.map((row) => ({
  //   ...row,
  //   getVisibleCells: () => row.getCenterVisibleCells(),
  // }));

  // const rightRowsData = table.getRowModel().rows.map((row) => ({
  //   ...row,
  //   getVisibleCells: () => row.getRightVisibleCells(),
  // }));

  // const hasPinningLeft = table.getState().columnPinning?.left?.length ?? 0 > 0;
  // const hasPinningRight = table.getState().columnPinning?.right?.length ?? 0 > 0;

  return (
    <Table
      aria-label="data-table"
      stickyHeader
      stickyFooter
      borderAxis="xBetween"
      hoverRow
      sx={{
        '--TableCell-headBackground': 'var(--joy-palette-background-level1)',
        '--TableRow-hoverBackground': 'var(--joy-palette-background-level1)',
        '& thead > tr > th.data-table-pinned-left': {
          zIndex: 'calc(var(--joy-zIndex-table) + 2)',
        },
        '& thead > tr > th.data-table-pinned-right': {
          zIndex: 'calc(var(--joy-zIndex-table) + 2)',
        },
        '& .data-table-pinned-left': {
          position: 'sticky',
          left: 0,
          zIndex: 'calc(var(--joy-zIndex-table) + 1)',
          backgroundColor: 'var(--TableCell-headBackground)',
          borderRight: '1px solid var(--TableCell-borderColor)',
        },
        '& .data-table-pinned-right': {
          position: 'sticky',
          right: 0,
          zIndex: 'calc(var(--joy-zIndex-table) + 1)',
          backgroundColor: 'var(--TableCell-headBackground)',
          borderLeft: '1px solid var(--TableCell-borderColor)',
        },
      }}
    >
      <THead headerGroups={table.getHeaderGroups()} />
      {isLoading && <LoadingOverlay table={table} />}
      {!isLoading && <TBody rows={table.getRowModel().rows} />}
    </Table>
  );
};

export default DataTable;
