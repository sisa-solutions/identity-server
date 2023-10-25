'use client';

import Button from '@mui/joy/Button';
import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import { FormActions, FormContainer, TextField, useForm, yup, yupResolver } from '@sisa/form';

import { DotIcon, LinkIcon, MailIcon } from 'lucide-react';

const ExternalLoginCallbackPage = () => {
  const validationSchema = yup.object({
    email: yup.string().required().email().min(6).max(50).label('Email'),
  });

  type FormValues = yup.InferType<typeof validationSchema>;

  const { control, handleSubmit } = useForm<FormValues>({
    defaultValues: {
      email: '',
    },
    resolver: yupResolver(validationSchema),
    reValidateMode: 'onBlur',
  });

  const onSubmit = handleSubmit((data: FormValues) => {
    console.log(data);
  });

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
          External login
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">
            {`You have successfully signed in with your external login provider.`}
          </Typography>
          <Typography level="body-sm">
            {`Please enter an email address to associate with your account.`}
          </Typography>
        </Card>
      </Stack>
      <FormContainer orientation="vertical">
        <TextField
          control={control}
          name="email"
          label="Email"
          type="email"
          required
          placeholder="Enter your email address"
          startDecorator={<MailIcon />}
        />
        <FormActions display="flex" flex={1} mt={2}>
          <Button
            type="submit"
            variant="solid"
            color="primary"
            sx={{ flex: 1 }}
            onClick={onSubmit}
            startDecorator={<LinkIcon />}
          >
            Associate
          </Button>
        </FormActions>
      </FormContainer>
    </Stack>
  );
};

export default ExternalLoginCallbackPage;
