'use client';

import { ReactNode } from 'react';

import Button from '@mui/joy/Button';
import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';
import Divider from '@mui/joy/Divider';
import ButtonGroup from '@mui/joy/ButtonGroup';

import { Link } from '@sisa/next';

interface Props {
  title: ReactNode;
  messages: Array<ReactNode>;
  children?: ReactNode;
  dividerText?: ReactNode;
  otherActions?: Array<{
    icon?: ReactNode;
    text: ReactNode;
  }>;
  footer?: {
    text: ReactNode;
    link?: string;
  };
}

const CardLayout = ({ title, messages, children, dividerText, otherActions, footer }: Props) => {
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
          {title}
        </Typography>
        <Card variant="soft">
          {messages.map((message, index) => (
            <Typography key={index} level="body-sm">
              {message}
            </Typography>
          ))}
        </Card>
      </Stack>

      {children && children}

      {dividerText && <Divider>{dividerText}</Divider>}

      {otherActions && (
        <ButtonGroup
          orientation="horizontal"
          spacing={2}
          sx={{
            '& > button': {
              flex: 1,
            },
          }}
          variant="solid"
          color="primary"
        >
          {otherActions.map((action, index) => (
            <Button
              key={index}
              {...(action.icon && {
                startDecorator: action.icon,
              })}
            >
              {action.text}
            </Button>
          ))}
        </ButtonGroup>
      )}

      {footer && (
        <Typography level="body-sm" textAlign="left" mt={2}>
          {footer.text}
          {footer.link && (
            <Link href="/register" color="primary" underline="always">
              {footer.link}
            </Link>
          )}
        </Typography>
      )}
    </Stack>
  );
};

export default CardLayout;
