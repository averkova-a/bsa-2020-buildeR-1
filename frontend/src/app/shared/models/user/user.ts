import { UserSocialNetwork } from './user-social-network';
import { BuildHistory } from '../build-history';

export interface User {
  id: number;
  role: string;
  email: string;
  username: string;
  firstName: string;
  lastName: string;
  bio: string;
  avatarUrl: string;
  createdAt: Date;
  userSocialNetworks: UserSocialNetwork[];
  buildHistories: BuildHistory[];
}
