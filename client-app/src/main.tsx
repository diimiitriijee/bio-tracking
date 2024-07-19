import React from 'react'
import ReactDOM from 'react-dom/client'
import 'semantic-ui-css/semantic.min.css'
import './index.css'
import 'semantic-ui-css/semantic.min.css'
import 'react-calendar/dist/Calendar.css'
import 'react-toastify/ReactToastify.min.css'
import 'react-datepicker/dist/react-datepicker.css'
import { StoreContext, store } from './App/stores/store.ts'
import { RouterProvider } from 'react-router-dom'
import { router } from './App/router/Routes.tsx'


ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <StoreContext.Provider value={store}>
      <RouterProvider router={router} />
    </StoreContext.Provider>
  </React.StrictMode>,
)
