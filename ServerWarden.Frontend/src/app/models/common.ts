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
}