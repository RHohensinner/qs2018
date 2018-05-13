using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics.Contracts;

namespace ContractExamples
{
    public class Purse
    {
        int MAX_BALANCE;
        int balance;
        byte[] pin;

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(balance >= 0 && balance <= MAX_BALANCE);

            Contract.Invariant(pin != null && pin.Length == 4);
            Contract.Invariant(Contract.ForAll(0, pin.Length, i => 0 <= pin[i] && pin[i] <= 9));
        }

        public Purse(int mb, int b, byte[] p)
        {
            Contract.Requires(0 < mb && 0 <= b && b <= mb);
            Contract.Requires(p != null && p.Length == 4);
            Contract.Requires(Contract.ForAll(0, p.Length, i => 0 <= p[i] && p[i] <= 9));

            //Contract.Ensures(0 < mb && 0 <= b && b <= mb);
            Contract.Ensures(MAX_BALANCE == mb && balance == b);
            Contract.Ensures(Contract.ForAll(0, p.Length, i => pin[i] == p[i]));

            MAX_BALANCE = mb; balance = b; pin = (byte[])p.Clone();
        }

        public bool checkPin(byte[] p)
        {
            Contract.Requires(p != null && p.Length == 4);

            Contract.Ensures(Contract.Result<bool>() == Contract.ForAll(0, p.Length, i => pin[i] == p[i]));

            bool res = true;
            for (int i = 0; i < 4; i++) { res = res && pin[i] == p[i]; }
            return res;
        }

        public int debit(int amount)
        {
            Contract.Requires(amount > 0);

            Contract.Ensures(Contract.Result<int>() == balance);
            Contract.Ensures(balance == Contract.OldValue<int>(balance) - amount);

            Contract.EnsuresOnThrow<Exception>(balance == Contract.OldValue<int>(balance));

            if (amount <= balance) { balance -= amount; return balance; }
            else
            {
                throw new Exception("Purse overdrawn by " + amount);
            }

        }

    }

}
