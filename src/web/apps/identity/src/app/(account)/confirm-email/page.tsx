import Button from '@mui/joy/Button';
import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import { CheckCheckIcon } from 'lucide-react';

const ConfirmEmailPage = () => {
  return (
    <Stack direction="column" gap={2}>
      <Stack
        direction="column"
        gap={1}
        sx={{
          mb: 2,
        }}
      >
        <Typography level="h3" color="primary">
          Confirm email
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">
            {`Please click the button below to confirm your email address.`}
          </Typography>
        </Card>
      </Stack>

      <Button
        type="submit"
        variant="solid"
        color="primary"
        startDecorator={<CheckCheckIcon />}
        sx={{ flex: 1 }}
      >
        Confirm email
      </Button>

      <Typography level="body-sm" mt={2}>
        {`If you didn't receive an email, please check your spam folder.`}
      </Typography>
    </Stack>
  );
};

export default ConfirmEmailPage;
