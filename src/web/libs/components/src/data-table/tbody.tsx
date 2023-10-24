import { Row } from '@tanstack/react-table';

import Box from '@mui/joy/Box';

import Td from './td';

type Props<T> = {
  rows: Array<Row<T>>;
};

const TBody = <T extends {}>(props: Props<T>) => {
  const { rows } = props;

  return (
    <Box component="tbody">
      {rows.map((row) => (
        <Box
          component="tr"
          key={row.id}
          sx={{
            ...(row.getIsSelected() && {
              backgroundColor: 'var(--TableCell-selectedBackground)',
            }),
          }}
        >
          {row.getVisibleCells().map((cell) => (
            <Td key={cell.id} cell={cell} />
          ))}
        </Box>
      ))}
    </Box>
  );
};

export default TBody;
