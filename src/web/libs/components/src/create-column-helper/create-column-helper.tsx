import {
  type ColumnHelper as ColumnHelperBase,
  type RowData,
  createColumnHelper as createColumnHelperBase,
} from '@tanstack/react-table';

import Checkbox from '@mui/joy/Checkbox';
import Chip, { ChipProps } from '@mui/joy/Chip';

import { CheckIcon, MinusIcon } from 'lucide-react';

import { ACTIONS_COLUMN_ID, FLEX_COLUMN_ID, SELECTION_COLUMN_ID } from '../data-table/constants';
import SettingsAction from '../data-table/settings-action';

type ColumnHelper<TData extends RowData> = ColumnHelperBase<TData> & {
  selection: (
    accessor: Parameters<ColumnHelperBase<TData>['accessor']>[0],
    column?: Parameters<ColumnHelperBase<TData>['accessor']>[1]
  ) => ReturnType<ColumnHelperBase<TData>['accessor']>;
  dangerouslyHtml: (
    accessor: Parameters<ColumnHelperBase<TData>['accessor']>[0],
    column?: Parameters<ColumnHelperBase<TData>['accessor']>[1]
  ) => ReturnType<ColumnHelperBase<TData>['accessor']>;
  actions: (
    accessor: Parameters<ColumnHelperBase<TData>['accessor']>[0],
    column?: Parameters<ColumnHelperBase<TData>['accessor']>[1]
  ) => ReturnType<ColumnHelperBase<TData>['accessor']>;
  colorCode: (
    accessor: Parameters<ColumnHelperBase<TData>['accessor']>[0],
    column?: Parameters<ColumnHelperBase<TData>['accessor']>[1] & {
      defaultValue?: string | number;
      colorMap: Record<string | number, ChipProps['color']>;
      sx: ChipProps['sx'];
    }
  ) => ReturnType<ColumnHelperBase<TData>['accessor']>;
  flex: () => ReturnType<ColumnHelperBase<TData>['accessor']>;
};

const createColumnHelper: <TData extends RowData>() => ColumnHelper<TData> = <
  TData extends RowData,
>() => {
  const helper = createColumnHelperBase<TData>();
  type accessorArgs = Parameters<(typeof helper)['accessor']>;
  type accessorArg = accessorArgs[0];
  type columnArg = accessorArgs[1];

  const selection = (accessor: accessorArg, column?: columnArg) =>
    helper.accessor(accessor, {
      id: SELECTION_COLUMN_ID,
      header: ({ table }) => (
        <Checkbox
          checkedIcon={<CheckIcon />}
          indeterminateIcon={<MinusIcon />}
          {...{
            checked: table.getIsAllRowsSelected(),
            indeterminate: table.getIsSomeRowsSelected(),
            onChange: table.getToggleAllRowsSelectedHandler(),
          }}
          sx={{
            verticalAlign: 'text-bottom',
            justifyContent: 'center',
            width: '100%',
          }}
        />
      ),
      cell: ({ row, getValue }) => (
        <Checkbox
          checkedIcon={<CheckIcon />}
          indeterminateIcon={<MinusIcon />}
          value={getValue() as any}
          {...{
            checked: row.getIsSelected(),
            indeterminate: row.getIsSomeSelected(),
            onChange: row.getToggleSelectedHandler(),
            disabled: !row.getCanSelect(),
          }}
          sx={{
            verticalAlign: 'text-bottom',
          }}
        />
      ),
      enableColumnFilter: false,
      enableResizing: false,
      enableGlobalFilter: false,
      enableSorting: false,
      enableMultiSort: false,
      enableGrouping: false,
      enableHiding: false,
      enablePinning: false,
      size: 52,
      minSize: 52,
      maxSize: 52,
      ...column,
    });

  const dangerouslyHtml = (accessor: accessorArg, column?: columnArg) =>
    helper.accessor(accessor, {
      // @ts-ignore
      id: column?.id,
      cell: ({ getValue }) => (
        <div dangerouslySetInnerHTML={{ __html: getValue<string>() ?? '' }} />
      ),
      ...column,
    });

  const colorCode = (
    accessor: accessorArg,
    column?: columnArg & {
      defaultValue?: string | number;
      colorMap: Record<string | number, ChipProps['color']>;
      sx: ChipProps['sx'];
    }
  ) => {
    const { defaultValue, colorMap = {}, sx, ...rest } = column ?? { defaultValue: '' };

    return helper.accessor(accessor, {
      // @ts-ignore
      id: column?.id,
      cell: ({ getValue }) => {
        const value = getValue<string | number>() ?? defaultValue;
        const color = colorMap[value] ?? 'primary';

        return (
          <Chip color={color} sx={sx}>
            {value}
          </Chip>
        );
      },
      ...rest,
    });
  };

  const actions = (accessor: accessorArg, column?: columnArg) =>
    helper.accessor(accessor, {
      id: ACTIONS_COLUMN_ID,
      enableColumnFilter: false,
      enableResizing: false,
      enableGlobalFilter: false,
      enableSorting: false,
      enableMultiSort: false,
      enableGrouping: false,
      enableHiding: false,
      enablePinning: false,
      size: 52,
      minSize: 52,
      maxSize: 52,
      header: ({ table }) => <SettingsAction table={table} />,
      ...column,
    });

  const flex = () => ({
    id: FLEX_COLUMN_ID,
    enableColumnFilter: false,
    enableResizing: false,
    enableGlobalFilter: false,
    enableSorting: false,
    enableMultiSort: false,
    enableGrouping: false,
    enableHiding: false,
    enablePinning: false,
    size: 0,
    minSize: 0,
    maxSize: 0,
  });

  return {
    ...helper,
    selection,
    colorCode,
    actions,
    dangerouslyHtml,
    flex,
  };
};

export default createColumnHelper;
