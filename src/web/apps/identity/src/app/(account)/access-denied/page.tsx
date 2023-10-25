import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import { AlertTriangleIcon } from 'lucide-react';

const AccessDeniedPage = () => {
  return (
    <Stack direction="column" gap={2}>
      <Stack
        direction="column"
        gap={1}
        sx={{
          mb: 2,
        }}
      >
        <Typography level="h3" color="warning" startDecorator={<AlertTriangleIcon />}>
          Access denied
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">{`You don't have access to this resource.`}</Typography>
        </Card>
      </Stack>
    </Stack>
  );
};

export default AccessDeniedPage;
