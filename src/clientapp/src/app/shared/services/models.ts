export abstract class MessageBase {
  public abstract getType(): string;
}

export interface IMessageSubscriber<T extends MessageBase> {
  onMessage(message: T): void;
  getType(): string;
}

export interface IdentityError {
  code: string;
  description: string;
}

export interface IdentityResult {
  succeeded: boolean;
  errors: Array<IdentityError>;
}

export class ResponseErrorHandler {
  public static handleError(error: Response): any {
    console.log(error || 'Server error');
    return error;
  }
}
