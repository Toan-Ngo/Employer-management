export class UserModel{
    id: string | undefined;
    email!: string;
    firstName!: string;
    roles!: string[];
    permissions: any;
}