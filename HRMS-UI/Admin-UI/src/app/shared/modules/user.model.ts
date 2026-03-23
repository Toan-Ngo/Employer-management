export class UserModel {
  id: string | null | undefined;
  email!: string;
  name?: string;
  firstName!: string;
  employeeCode?: string;
  roles!: string[];
  permissions: any;
  accessToken!: string;
}
