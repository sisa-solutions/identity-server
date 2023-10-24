import { HeaderGroup } from '@tanstack/react-table';

import Th from './th';
import Box from '@mui/joy/Box';

type Props<T> = {
  headerGroups: Array<HeaderGroup<T>>;
};

const THead = <T extends {}>(props: Props<T>) => {
  const { headerGroups } = props;

  return (
    <Box component="thead">
      {headerGroups.map((headerGroup) => (
        <Box
          key={headerGroup.id}
          component="tr"
          sx={{
            height: 48,
          }}
        >
          {headerGroup.headers.map((header) => (
            <Th key={header.id} header={header} />
          ))}
        </Box>
      ))}
    </Box>
  );
};

export default THead;
