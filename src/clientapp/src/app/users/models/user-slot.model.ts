import {
  Slot,
  SINGLE_SLOT,
  Editor,
  HIDDEN_EDITOR,
  TEXT_EDITOR,
  CHECKBOX_EDITOR,
  MULTI_SELECT_EDITOR,
  VALUE_BINDING_BEHAVIOR
} from '../../shared/formdef';

export class UserDetailSlot implements Slot {
  public static KEY = 'UserDetailSlot';

  public key = UserDetailSlot.KEY;
  public type = SINGLE_SLOT;
  public title = 'Detail';
  public editors: Editor[];

  public constructor(
    claims: Array<string>,
    roles: Array<string>
  ) {
    const claimOptions = claims.map((a: string) => {
      return { key: a, value: a };
    });

    const roleOptions = roles.map((a: string) => {
      return { key: a, value: a };
    });

    this.editors = [
      {
        key: 'id',
        type: HIDDEN_EDITOR,
        label: 'Id',
        required: true
      },
      {
        key: 'userName',
        type: TEXT_EDITOR,
        label: 'Name',
        required: true
      },
      {
        key: 'email',
        type: TEXT_EDITOR,
        label: 'Email',
        required: true
      },
      {
        key: 'lockoutEnabled',
        type: CHECKBOX_EDITOR,
        label: 'Lockout enabled'
      },
      {
        key: 'isLockedOut',
        type: CHECKBOX_EDITOR,
        label: 'Is locked out',
        isReadOnly: true
      },
      {
        key: 'claims',
        type: MULTI_SELECT_EDITOR,
        label: 'Claims',
        required: false,
        options: claimOptions,
        singleSelection: false,
        bindingBehaviour: VALUE_BINDING_BEHAVIOR
      },
      {
        key: 'roles',
        type: MULTI_SELECT_EDITOR,
        label: 'Roles',
        required: false,
        options: roleOptions,
        singleSelection: false,
        bindingBehaviour: VALUE_BINDING_BEHAVIOR
      }
    ];
  }
}
