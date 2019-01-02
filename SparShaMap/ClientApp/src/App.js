import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import MapAdmin from './components/MapAdmin';
import Index from './components/Index';
import User from './components/User';
import Promotion from './components/Promotion';
import Ads from './components/Ads';
import Notification from './components/Notification';
import Login from './components/Login';
import MapMember from './components/MapMember';
export default () => (
    <div>
        <Layout>
            <Route exact path='/' component={Index} />
            <Route path='/Users/:startDateIndex?' component={User} />
            <Route path='/Ads/:startDateIndex?' component={Ads} />
            <Route path='/Promotion/:startDateIndex?' component={Promotion} />
            <Route path='/Notification/:startDateIndex?' component={Notification} />
            <Route path='/Login' component={Login} />
        </Layout>
        <Route path='/MapAdmin' component={MapAdmin} />
        <Route path='/MapMember' component={MapMember} />
    </div>
);
