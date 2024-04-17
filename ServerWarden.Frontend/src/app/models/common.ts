export interface ApiResponse {
  success: boolean,
  code: ResultCode,
  message: string,
  data: any,
}

export enum ResultCode
{
  Success,
  InvalidParameters,
  Failure,

  InvalidNewPassword,
  InvalidNewUsername,
  InvalidPassword,
  UserExists,
  UserNotFound,
  UserNotAuthorized,

  ServerNotFound,
  InvalidServerType,
  ServerAlreadyInstalled,
  ServerIsInstalling,
  ServerNotInstalled,
}

export enum ModalType{
  NewServer
}

export interface DropdownOption{
  id: number,
  name: string,
}