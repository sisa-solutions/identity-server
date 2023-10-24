import Button, { ButtonProps } from '@mui/joy/Button';
import Divider from '@mui/joy/Divider';

import DialogTitle from '@mui/joy/DialogTitle';
import DialogContent from '@mui/joy/DialogContent';
import DialogActions from '@mui/joy/DialogActions';

import Modal, { type ModalProps } from '@mui/joy/Modal';
import ModalDialog, { type ModalDialogProps } from '@mui/joy/ModalDialog';
import { LinearProgress } from '@mui/joy';

type confirmDialogProps = Pick<
  ModalProps,
  | 'open'
  | 'onClose'
  | 'disablePortal'
  | 'disableEscapeKeyDown'
  | 'disableScrollLock'
  | 'hideBackdrop'
  | 'keepMounted'
> &
  Omit<ModalDialogProps, 'children' | 'component'> & {
    isLoading?: boolean;
    icon?: React.ReactNode;
    title: React.ReactNode;
    content: React.ReactNode;
    confirmProps: ButtonProps & {
      label: React.ReactNode;
    };
    cancelProps: ButtonProps & {
      label: React.ReactNode;
    };
  };

const ConfirmDialog = ({
  open,
  isLoading,
  onClose,
  disablePortal,
  disableEscapeKeyDown,
  disableScrollLock,
  hideBackdrop,
  keepMounted,
  icon,
  title,
  content,
  confirmProps,
  cancelProps,
  ...modalProps
}: confirmDialogProps) => {
  const { label: confirmLabel, ...restConfirmProps } = confirmProps;
  const { label: cancelLabel, ...restCancelProps } = cancelProps;

  return (
    <Modal
      open={open}
      onClose={onClose}
      disablePortal={disablePortal}
      disableEscapeKeyDown={disableEscapeKeyDown}
      disableScrollLock={disableScrollLock}
      hideBackdrop={hideBackdrop}
      keepMounted={keepMounted}
    >
      <ModalDialog role="confirm-dialog" {...modalProps}>
        <DialogTitle>
          {icon && icon}
          {title}
        </DialogTitle>
        {isLoading ? (
          <LinearProgress thickness={1} color={modalProps.color ?? 'danger'} />
        ) : (
          <Divider />
        )}
        <DialogContent>{content}</DialogContent>
        <DialogActions>
          <Button
            variant="solid"
            color="danger"
            {...restConfirmProps}
            loading={isLoading}
            disabled={isLoading}
          >
            {confirmLabel}
          </Button>
          <Button variant="plain" color="neutral" {...restCancelProps}>
            {cancelLabel}
          </Button>
        </DialogActions>
      </ModalDialog>
    </Modal>
  );
};

export default ConfirmDialog;
