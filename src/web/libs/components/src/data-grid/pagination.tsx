import Box from '@mui/joy/Box';
import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import Select from '@mui/joy/Select';
import Option from '@mui/joy/Option';
import Typography from '@mui/joy/Typography';
import Input from '@mui/joy/Input';
import IconButton from '@mui/joy/IconButton';

import {
  ChevronLeftIcon,
  ChevronRightIcon,
  ChevronsLeftIcon,
  ChevronsRightIcon,
  ChevronsUpDownIcon,
} from 'lucide-react';

type PaginationProps = {
  pageIndex: number;
  pageSize: number;
  itemCount: number;
  pageCount: number;
  pageSizeOptions?: number[];

  onPageIndexChange: (pageIndex: number) => void;
  onPageSizeChange: (pageSize: number) => void;
  nextPage: () => void;
  previousPage: () => void;
};

const Pagination = ({
  pageIndex,
  pageSize,
  itemCount,
  pageCount,
  pageSizeOptions = [5, 10, 25, 50, 100],

  onPageIndexChange,
  onPageSizeChange,
  nextPage,
  previousPage,
}: PaginationProps) => {
  const pageNumber = pageIndex + 1;

  const fromRow = pageIndex * pageSize + (itemCount === 0 ? 0 : 1);
  const toRow = Math.min(pageNumber * pageSize, itemCount);

  const hasNextPage = () => pageNumber < pageCount;
  const hasPreviousPage = () => pageNumber > 1;

  return (
    <Box
      sx={{
        display: 'flex',
        gap: 2,
        alignItems: 'center',
      }}
    >
      <Typography
        sx={{
          textAlign: 'center',
          minWidth: 80,
        }}
      >
        {fromRow} - {toRow} of {itemCount}
      </Typography>
      <Box
        sx={{
          flexGrow: 1,
        }}
      />
      <FormControl
        orientation="horizontal"
        size="sm"
        sx={{
          display: {
            xs: 'none',
            md: 'inherit',
          },
        }}
      >
        <FormLabel
          sx={{
            fontSize: 'md',
          }}
        >
          Rows per page:
        </FormLabel>
        <Select
          indicator={<ChevronsUpDownIcon />}
          sx={{
            width: 80,
            fontSize: 'md',
          }}
          slotProps={{
            button: {
              sx: {
                textAlign: 'center',
              },
            },
          }}
          value={pageSize}
          onChange={(_, value) => {
            onPageSizeChange(Number(value));
          }}
        >
          {pageSizeOptions.map((value) => (
            <Option key={value} value={value}>
              {value}
            </Option>
          ))}
        </Select>
      </FormControl>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'row',
          gap: 1,
        }}
      >
        <IconButton
          size="sm"
          variant="outlined"
          color="neutral"
          disabled={!hasPreviousPage()}
          onClick={() => onPageIndexChange(0)}
        >
          <ChevronsLeftIcon />
        </IconButton>
        <IconButton
          size="sm"
          variant="outlined"
          color="neutral"
          disabled={!hasPreviousPage()}
          onClick={() => previousPage()}
        >
          <ChevronLeftIcon />
        </IconButton>
        <Input
          size="sm"
          type="number"
          sx={{
            width: 40,
            display: {
              xs: 'none',
              md: 'inherit',
            },
          }}
          value={pageNumber}
          slotProps={{
            input: {
              sx: {
                textAlign: 'center',
                '&::-webkit-inner-spin-button, &::-webkit-outer-spin-button': {
                  appearance: 'none',
                },
                MozAppearance: 'textfield',
              },
            },
          }}
          onChange={(e) => {
            const newPageIndex = e.target.value ? Number(e.target.value) - 1 : 0;

            onPageIndexChange(newPageIndex);
          }}
        />
        <IconButton
          size="sm"
          variant="outlined"
          color="neutral"
          disabled={!hasNextPage()}
          onClick={() => nextPage()}
        >
          <ChevronRightIcon />
        </IconButton>
        <IconButton
          size="sm"
          variant="outlined"
          color="neutral"
          disabled={!hasNextPage()}
          onClick={() => onPageIndexChange(pageCount - 1)}
        >
          <ChevronsRightIcon />
        </IconButton>
      </Box>
    </Box>
  );
};

export default Pagination;
