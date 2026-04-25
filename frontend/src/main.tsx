import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import { AgendamentoProvider } from './AgendamentoContext.tsx'
import './index.css'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AgendamentoProvider>
      <App />
    </AgendamentoProvider>
  </React.StrictMode>,
)
