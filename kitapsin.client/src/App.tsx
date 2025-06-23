import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './components/AuthContext';
import PrivateRoute from './PrivateRoute';
import AppLayout from './AppLayout';
import Home from './pages/Home';
import About from './pages/About';
import Login from './pages/Login';
import BooksPage from './pages/Books';
import Categories from './pages/Categories';
import AddCategory from './pages/AddPages/AddCategory'; 
import UpdateCategory from './pages/UpdatePages/UpdateCategory';
import Publishers from './pages/Publishers';
import AddPublisher from './pages/AddPages/AddPublisher';
import UpdatePublisher from './pages/UpdatePages/UpdatePublisher';
import Authors from './pages/Authors';
import AddAuthor from './pages/AddPages/AddAuthor';
import UpdateAuthor from './pages/UpdatePages/UpdateAuthor';
import Users from './pages/Users';
import AddUser from './pages/AddPages/AddUser';
import UpdateUser from './pages/UpdatePages/UpdateUser';
import AddBook from './pages/AddPages/AddBook';
import UpdateBook from './pages/UpdatePages/UpdateBook';
import RentBook from './pages/RentBook'
import AddPenalty from './pages/AddPages/AddPenalty';
import Shelf from './pages/Shelf';

const App: React.FC = () => (
    <AuthProvider>
        <Router>
            <Routes>
                <Route path="/login" element={<Login />} />

                {/* Private route içinde ortak layout */}
                <Route
                    path="/"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <Home />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/about"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <About />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/Books"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <BooksPage />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Users/Loans/RentBook/:id"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <RentBook />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Books/AddBook"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <AddBook />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Books/UpdateBook/:id"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <UpdateBook />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/categories"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <Categories />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Shelf"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <Shelf />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />

                <Route
                    path="/categories/add"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <AddCategory />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/categories/update/:id"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <UpdateCategory />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Publishers"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <Publishers />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Publishers/AddPublisher"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <AddPublisher/>
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Publishers/update/:id"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <UpdatePublisher/>
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Authors"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <Authors />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Authors/AddAuthor"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <AddAuthor />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Authors/UpdateAuthor/:id"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <UpdateAuthor />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Users"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <Users />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Users/AddUser"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <AddUser />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Users/UpdateUser/:id"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <UpdateUser />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Users/:id/Penalties/AddPenalty"
                    element={
                        <PrivateRoute>
                            <AppLayout>
                                <AddPenalty />
                            </AppLayout>
                        </PrivateRoute>
                    }
                />
            </Routes>
        </Router>
    </AuthProvider>
);

export default App;