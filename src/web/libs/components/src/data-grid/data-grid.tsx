'use client';

import { useEffect, useState } from 'react';

import type {
  ColumnOrderState,
  ColumnPinningState,
  PaginationState,
  RowSelectionState,
  SortingState,
} from '@tanstack/react-table';

import Box from '@mui/joy/Box';
import Sheet from '@mui/joy/Sheet';

import DataTable, { type DataTableProps } from '../data-table/data-table';

import Toolbar from './toolbar';
import Pagination from './pagination';

import { useQueryString } from '@sisa/hooks';
import { sortParamsToString } from '@sisa/utils';

export type DataGridProps<T extends {}> = DataTableProps<T> & {
  pageIndex: number;
  pageSize: number;

  pageSizeOptions?: number[];

  syncExternalState?: (state: Partial<Record<string, string | number>>) => void;
  serializeOrderState?: (state: SortingState) => string;
};

const DataGrid = <T extends {}>({
  columns,
  data,
  pageIndex,
  pageSize,
  itemCount,
  pageCount = 0,
  pageSizeOptions = [5, 10, 25, 50, 100],
  syncExternalState,
  serializeOrderState,
  slots = {
    toolbar: null,
  },
  ...restProps
}: DataGridProps<T>) => {
  const setQueryString = useQueryString();
  const syncUrlState = syncExternalState ?? setQueryString;
  const sortStringToParams = serializeOrderState ?? sortParamsToString;

  const [pagination, setPagination] = useState<PaginationState>(() => ({
    pageIndex: pageIndex,
    pageSize: pageSize,
  }));

  const [rowSelection, setRowSelection] = useState<RowSelectionState>({});
  const [sorting, setSorting] = useState<SortingState>([]);
  const [columnPinning, setColumnPinning] = useState<ColumnPinningState>({
    left: ['__SELECTION__'],
    right: ['__ACTIONS__'],
  });

  const [columnOrder, setColumnOrder] = useState<ColumnOrderState>(
    columns.map((column) => column.id as string)
  );

  useEffect(() => {
    syncUrlState({
      pageNumber: pagination.pageIndex === 0 ? undefined : pagination.pageIndex + 1,
      pageSize: pagination.pageSize === 10 ? undefined : pagination.pageSize,
    });
  }, [pagination.pageIndex, pagination.pageSize]);

  useEffect(() => {
    syncUrlState({
      sortBy: sortStringToParams(sorting),
    });
  }, [sorting]);

  const onPageIndexChange = (pageIndex: number) => {
    if (pageIndex < 0 || pageIndex >= pageCount) return;

    setPagination({ ...pagination, pageIndex });
  };

  const onPageSizeChange = (pageSize: number) => {
    if (!pageSizeOptions?.includes(pageSize)) return;

    setPagination({ ...pagination, pageSize });
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        gap: 1,
        overflow: 'hidden',
      }}
    >
      <Toolbar>{slots.toolbar}</Toolbar>
      <Sheet
        variant="outlined"
        className="table-container"
        sx={{
          width: '100%',
          borderRadius: 'sm',
          flex: 1,
          overflow: 'auto',
          minHeight: 0,
        }}
      >
        <DataTable
          columns={columns}
          data={data}
          itemCount={itemCount}
          pageCount={pageCount}
          enableRowSelection
          state={{
            pagination,
            rowSelection,
            sorting,
            columnPinning,
            columnOrder,
          }}
          onPaginationChange={setPagination}
          onRowSelectionChange={setRowSelection}
          onSortingChange={setSorting}
          onColumnPinningChange={setColumnPinning}
          onColumnOrderChange={setColumnOrder}
          {...restProps}
        />
      </Sheet>
      <Pagination
        pageIndex={pagination.pageIndex}
        pageSize={pagination.pageSize}
        itemCount={itemCount}
        pageCount={pageCount}
        pageSizeOptions={pageSizeOptions}
        onPageIndexChange={onPageIndexChange}
        onPageSizeChange={onPageSizeChange}
        nextPage={() => setPagination({ ...pagination, pageIndex: pagination.pageIndex + 1 })}
        previousPage={() => setPagination({ ...pagination, pageIndex: pagination.pageIndex - 1 })}
      />
    </Box>
  );
};

export default DataGrid;
